using System;
using System.IO;
using System.Threading;

namespace WindowsFormsApplication4
{
	internal static class StreamExtensions
	{
		public static IAsyncResult BeginReadToEnd(this Stream stream, 
			byte[] buffer, int offset, int count, AsyncCallback callback, Object state)
		{
			byte[] tempBuffer = new byte[count];
			ByteArrayAsyncResult result = new ByteArrayAsyncResult(callback, state, buffer, offset, tempBuffer);
			ByteArrayAsyncState asyncState = new ByteArrayAsyncState {Result = result, Stream = stream};
			stream.BeginRead(tempBuffer, 0, count, OnRead, asyncState);
			return result;
		}

		public static int EndReadToEnd(this Stream stream, IAsyncResult ar)
		{
			ByteArrayAsyncResult state = ar as ByteArrayAsyncResult;
			if (state == null) throw new InvalidOperationException();
			state.AsyncWaitHandle.WaitOne(-1);
			return state.Length;
		}

		private static void OnRead(IAsyncResult ar)
		{
			if (ar == null) return;
			ByteArrayAsyncState state = ar.AsyncState as ByteArrayAsyncState;
			if (state == null) return;
			int bytesRead = state.Stream.EndRead(ar);
			if (bytesRead != 0)
			{
				Array.Copy(state.Result.TempBuffer, 0, state.Result.Result, state.Result.Index, bytesRead);
				state.Result.Index += bytesRead;
				state.Result.Length = state.Result.Index;
				state.Stream.BeginRead(state.Result.TempBuffer, 0, state.Result.Result.Length - state.Result.Length, OnRead, state);
				return;
			}
			state.Result.Complete(false);
		}

		private class ByteArrayAsyncResult : IAsyncResult, IDisposable
		{
			private readonly AsyncCallback callback_;
			private bool completed_;
			private bool completedSynchronously_;
			private readonly object asyncState_;
			private readonly ManualResetEvent waitHandle_;
			private Exception e_;
			private readonly object syncRoot_;
			public byte[] Result;
			public int Index;
			public byte[] TempBuffer;
			public int Length;

			internal ByteArrayAsyncResult(AsyncCallback cb, object state, byte[] buffer, int offset, byte[] tempBuffer)
				: this(cb, state, buffer, offset, tempBuffer, false)
			{
			}

			private ByteArrayAsyncResult(AsyncCallback cb, object state, byte[] buffer, int offset, byte[] tempBuffer, bool completed)
			{
				this.callback_ = cb;
				this.asyncState_ = state;
				this.completed_ = completed;
				this.completedSynchronously_ = completed;
				this.Result = buffer;
				this.Index = offset;
				this.TempBuffer = tempBuffer;

				this.waitHandle_ = new ManualResetEvent(false);
				this.syncRoot_ = new object();
			}

			public object AsyncState
			{
				get { return this.asyncState_; }
			}

			public WaitHandle AsyncWaitHandle
			{
				get { return this.waitHandle_; }
			}

			public bool CompletedSynchronously
			{
				get
				{
					lock (this.syncRoot_)
					{
						return this.completedSynchronously_;
					}
				}
			}

			public bool IsCompleted
			{
				get
				{
					lock (this.syncRoot_)
					{
						return this.completed_;
					}
				}
			}

			public void Dispose()
			{
				this.Dispose(true);
				GC.SuppressFinalize(this);
			}

			protected virtual void Dispose(bool disposing)
			{
				if (disposing)
				{
					lock (this.syncRoot_)
					{
						if (this.waitHandle_ != null)
						{
							((IDisposable)this.waitHandle_).Dispose();
						}
					}
				}
			}

			internal void Complete(bool completedSynchronously)
			{
				lock (this.syncRoot_)
				{
					this.completed_ = true;
					this.completedSynchronously_ =
						completedSynchronously;
				}

				this.SignalCompletion();
			}

			private void SignalCompletion()
			{
				this.waitHandle_.Set();

				ThreadPool.QueueUserWorkItem(new WaitCallback(this.InvokeCallback));
			}

			private void InvokeCallback(object state)
			{
				if (this.callback_ != null)
				{
					this.callback_(this);
				}
			}
		}

		private class ByteArrayAsyncState
		{
			public ByteArrayAsyncResult Result;
			public Stream Stream;
		}
	}
}