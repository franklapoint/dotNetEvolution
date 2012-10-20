VB versions of projects are even-numbered.
##.NET 1.1:##
#01 WindowsFormsApplication#
Introduces the sample application, asynchronous methods (Begin/End), APM
Explicit delegates (new DelegateType(...) Control.InvokeRequired explanation, detail StreamHelper
#02 WindowsApplication#
Introduces the sample application, asynchronous methods (Begin/End), APM

##.NET 2.0:##
#03 WindowsFormsApplication#
Introduction of partial types, anoymous methods (anoymous delegates)
#04 WindowsApplication#
only partial types, VB doesn't get anonymous methods yet
#05 WindowsFormsApplication#
Generics in GetScriptBodies, GetScriptBodies becomes iterator method
#05b WindowsFormsApplication#
introduce progress and cancellation, using WebClient instead of WebRequest, introduction of EAP and how Webclient implements EAP
#05c WindowsFormsApplication#
Progress and cancellation with BackgroundWorker as example (not optimal use of BG, but correlates to previous examples), "fake" progress, lazy code monkey
#06 WindowsApplication#
VB version of 05, sans iterator methods: VB doesn't get this in .NET 2.0
#06b WindowsApplication#
VB version of 05b
#06c WindowsApplication#
VB version of 05c

##.NET 3.0:##
#07 WindowsFormsApplication#
var, problems with foreach(Match..., lambdas, use of expression lambda in BeginGetResponse and statement lambda elsewhere.  encapsulation of asynchronous "callbacks" within the method that makes the async call.  Extension methods and StreamExtensions
#08 WindowsApplication#
VB version of 07
#09 WindowsFormsApplication#
LINQ in GetScriptBodies
#10 WindowsApplication#
VB version of 09

##.NET 4.0##
#11 WindowsFormsApplication#
Task and Task Parallel Library (TPL), TaskScheduler and FromCurrentSynchronizationContext, no need for InvokeRequired/BeginInvoke, more abstract code and can be used in WPF, etc. EnableButton gone
#11b WindowsFormsApplication#
Cancellation and progress with TPL, "fake" progress, nothing particular to TPL for progress, detail cancellation token source and token and how only token is shared--by value--across threads
#12 WindowsApplication#
VB version of 11
#12b WindowsApplication#
VB version of 11 b

##.NET 4.5##
#13 WindowsFormsApplication#
async/await, detail lines after await are "continuations": compare to ContinueWith code, detail how much less code there is from 01, no need for stream extension, use of "using"... detail automatic marshalling to UI thread
#13b WindowsFormsApplication#
progress with async/await.  CancellationTokenSource etc and now IProgress<T> and Progress<T> implementation.
#14 WindowsApplication#
VB version of 13
#14b WindowsApplication#
VB version of 13b
