Imports System.IO
Imports System.Threading
Imports System.Runtime.CompilerServices

Module StreamExtensions
    <Extension()> _
    Public Function BeginReadToEnd(ByVal stream As Stream, ByVal buffer As Byte(), ByVal offset As Integer, ByVal count As Integer, ByVal callback As AsyncCallback, ByVal state As Object) As IAsyncResult
        Dim tempBuffer(count) As Byte
        Dim result As ByteArrayAsyncResult = New ByteArrayAsyncResult(callback, state, buffer, offset, tempBuffer)
        Dim asyncState As ByteArrayAsyncState = New ByteArrayAsyncState()
        asyncState.Result = result
        asyncState.Stream = stream
        Dim onReadCallback As New AsyncCallback(AddressOf OnRead)
        stream.BeginRead(tempBuffer, 0, count, onReadCallback, asyncState)
        Dim asyncResult As IAsyncResult = result
        Return asyncResult
    End Function

    <Extension()> _
    Public Function EndReadToEnd(ByVal stream As Stream, ByVal ar As IAsyncResult) As Integer
        Dim state As ByteArrayAsyncResult = TryCast(ar, ByteArrayAsyncResult)
        If (Not state Is Nothing) Then
            state.AsyncWaitHandle.WaitOne(-1)
            Dim length As Integer = state.Length
            Return length
        Else
            Throw New InvalidOperationException()
        End If
    End Function

    Private Sub OnRead(ByVal ar As IAsyncResult)
        If (Not ar Is Nothing) Then
            Dim state As ByteArrayAsyncState = TryCast(ar.AsyncState, ByteArrayAsyncState)
            If (Not state Is Nothing) Then
                Dim bytesRead As Integer = state.Stream.EndRead(ar)
                If (bytesRead = 0) Then
                    state.Result.Complete(False)
                Else
                    Array.Copy(state.Result.TempBuffer, 0, state.Result.Result, state.Result.Index, bytesRead)
                    Dim result As ByteArrayAsyncResult = state.Result
                    result.Index = result.Index + bytesRead
                    state.Result.Length = state.Result.Index
                    Dim onReadCallback As New AsyncCallback(AddressOf OnRead)
                    state.Stream.BeginRead(state.Result.TempBuffer, 0, CInt(state.Result.Result.Length) - state.Result.Length, onReadCallback, state)
                End If
            End If
        End If
    End Sub

    Private Class ByteArrayAsyncResult
        Implements IAsyncResult, IDisposable
        Private ReadOnly asyncState_ As Object

        Private ReadOnly callback_ As AsyncCallback

        Private completed_ As Boolean

        Private completedSynchronously_ As Boolean

        Private e_ As Exception

        Public Index As Integer

        Public Length As Integer

        Public Result As Byte()

        Private ReadOnly syncRoot_ As Object

        Public TempBuffer As Byte()

        Private ReadOnly waitHandle_ As ManualResetEvent

        Public ReadOnly Property AsyncState As Object Implements IAsyncResult.AsyncState
            Get
                Dim obj As Object = Me.asyncState_
                Return obj
            End Get
        End Property

        Public ReadOnly Property AsyncWaitHandle As WaitHandle Implements IAsyncResult.AsyncWaitHandle
            Get
                Dim waitHandle As WaitHandle = Me.waitHandle_
                Return waitHandle
            End Get
        End Property

        Public ReadOnly Property CompletedSynchronously As Boolean Implements IAsyncResult.CompletedSynchronously
            Get
                Dim flag As Boolean
                SyncLock Me.syncRoot_
                    flag = Me.completedSynchronously_
                End SyncLock
                Return flag
            End Get
        End Property

        Public ReadOnly Property IsCompleted As Boolean Implements IAsyncResult.IsCompleted
            Get
                Dim flag As Boolean
                SyncLock Me.syncRoot_
                    flag = Me.completed_
                End SyncLock
                Return flag
            End Get
        End Property

        Friend Sub New(ByVal cb As AsyncCallback, ByVal state As Object, ByVal buffer As Byte(), ByVal offset As Integer, ByVal tempBuffer As Byte())
            MyClass.New(cb, state, buffer, offset, tempBuffer, False)
        End Sub

        Private Sub New(ByVal cb As AsyncCallback, ByVal state As Object, ByVal buffer As Byte(), ByVal offset As Integer, ByVal tempBuffer As Byte(), ByVal completed As Boolean)
            MyBase.New()
            Me.callback_ = cb
            Me.asyncState_ = state
            Me.completed_ = completed
            Me.completedSynchronously_ = completed
            Me.Result = buffer
            Me.Index = offset
            Me.TempBuffer = tempBuffer
            Me.waitHandle_ = New ManualResetEvent(False)
            Me.syncRoot_ = New Object()
        End Sub

        Friend Sub Complete(ByVal completedSynchronously As Boolean)
            SyncLock Me.syncRoot_
                Me.completed_ = True
                Me.completedSynchronously_ = completedSynchronously
            End SyncLock
            Me.SignalCompletion()
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            Me.Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub

        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If (disposing) Then
                SyncLock Me.syncRoot_
                    If (Not Me.waitHandle_ Is Nothing) Then
                        Dim disposable As IDisposable = waitHandle_
                        disposable.Dispose()
                    End If
                End SyncLock
            End If
        End Sub

        Private Sub InvokeCallback(ByVal state As Object)
            If (Not Me.callback_ Is Nothing) Then
                Me.callback_(Me)
            End If
        End Sub

        Private Sub SignalCompletion()
            Me.waitHandle_.Set()
            Dim waitCallback As New WaitCallback(AddressOf InvokeCallback)
            ThreadPool.QueueUserWorkItem(waitCallback)
        End Sub
    End Class

    Private Class ByteArrayAsyncState
        Public Result As ByteArrayAsyncResult

        Public Stream As Stream

        Public Sub New()
            MyBase.New()
        End Sub
    End Class
End Module

