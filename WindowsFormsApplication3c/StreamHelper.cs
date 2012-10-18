using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;

namespace WindowsFormsApplication3c
{
	internal class StreamHelper
	{
		public static IAsyncResult BeginReadStreamToEnd(Stream stream, byte[] buffer, int offset, int count, 
			AsyncCallback callback, Object state)
		{
			byte[] tempBuffer = new byte[count];
			ByteArrayAsyncResult result = new ByteArrayAsyncResult(callback, state, buffer, offset, tempBuffer);
			ByteArrayAsyncState asyncState = new ByteArrayAsyncState();
			asyncState.Result = result;
			asyncState.Stream = stream;
			stream.BeginRead(tempBuffer, 0, count, OnRead, asyncState);
			return result;
		}

		public static int EndReadStreamToEnd(IAsyncResult ar)
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
			try
			{
				int bytesRead = state.Stream.EndRead(ar);
				if (bytesRead != 0)
				{
					Array.Copy(state.Result.TempBuffer, 0, state.Result.Result, state.Result.Index, bytesRead);
					state.Result.Index += bytesRead;
					state.Result.Length = state.Result.Index;
					try
					{
						state.Stream.BeginRead(state.Result.TempBuffer, 0, state.Result.Result.Length - state.Result.Length, OnRead, state);
					}
					catch (WebException ex)
					{
						if (ex.Status == WebExceptionStatus.RequestCanceled) return;
						Trace.WriteLine(ex.Message);
						throw;
					}
					return;
				}
			}
			catch(ObjectDisposedException)
			{
				Trace.WriteLine("object disposed");
				// object disposed exception means abort
			}
			catch(WebException webException)
			{
				if (webException.Status != WebExceptionStatus.RequestCanceled) throw;
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
			private readonly object syncRoot_;
			public byte[] Result;
			public int Index;
			public byte[] TempBuffer;
			public int Length;

			internal ByteArrayAsyncResult(AsyncCallback cb, object state, byte[] buffer, int offset, byte[] tempBuffer)
				: this(cb, state, buffer, offset, tempBuffer, false)
			{
			}

			private ByteArrayAsyncResult(AsyncCallback cb, object state, byte[] buffer, int offset, 
				byte[] tempBuffer, bool completed)
			{
				callback_ = cb;
				asyncState_ = state;
				completed_ = completed;
				completedSynchronously_ = completed;
				Result = buffer;
				Index = offset;
				TempBuffer = tempBuffer;

				waitHandle_ = new ManualResetEvent(false);
				syncRoot_ = new object();
			}

			public object AsyncState
			{
				get { return asyncState_; }
			}

			public WaitHandle AsyncWaitHandle
			{
				get { return waitHandle_; }
			}

			public bool CompletedSynchronously
			{
				get
				{
					lock (syncRoot_)
					{
						return completedSynchronously_;
					}
				}
			}

			public bool IsCompleted
			{
				get
				{
					lock (syncRoot_)
					{
						return completed_;
					}
				}
			}

			public void Dispose()
			{
				Dispose(true);
				GC.SuppressFinalize(this);
			}

			protected virtual void Dispose(bool disposing)
			{
				if (disposing)
				{
					lock (syncRoot_)
					{
						if (waitHandle_ != null)
						{
							((IDisposable)waitHandle_).Dispose();
						}
					}
				}
			}

			internal void Complete(bool completedSynchronously)
			{
				lock (syncRoot_)
				{
					completed_ = true;
					completedSynchronously_ =
						completedSynchronously;
				}

				SignalCompletion();
			}

			private void SignalCompletion()
			{
				waitHandle_.Set();

				ThreadPool.QueueUserWorkItem(new WaitCallback(InvokeCallback));
			}

			private void InvokeCallback(object state)
			{
				if (callback_ != null)
				{
					callback_(this);
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