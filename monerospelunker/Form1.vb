Imports System
Imports System.IO

Public Class Form1

    Dim oProcess As New Process()

    Private Sub ButtonStart_Click(sender As Object, e As EventArgs) Handles ButtonStart.Click
        Dim xmr_address = TextBoxXmrAddress.Text()
        Dim path As String = Directory.GetCurrentDirectory()
        'MessageBox.Show(path, "Working directory", MessageBoxButtons.OKCancel)
        If xmr_address.Length = 95 And xmr_address.Substring(0, 1) = "4" Then
            BackgroundWorker1.WorkerSupportsCancellation = True
            BackgroundWorker1.RunWorkerAsync()
        Else
            MessageBox.Show("Address looks wrong!", "Address status", MessageBoxButtons.OKCancel)
        End If
    End Sub

    Private Sub BackgroundWorker1_DoWork(sender As Object, e As ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork

        Dim pool_address = TextBoxPoolAddress.Text()
        Dim xmr_address = TextBoxXmrAddress.Text()
        Dim threads = TextBoxThreads.Text()

        Dim sOutput As String
        Dim splitOutput() As String
        Dim shares As Int32
        Dim badshares As Int32
        Dim hashrate As String
        Dim cntr As Int16 = 0


        Dim oStartInfo As New ProcessStartInfo("cpuminer-multi-wolf-AES\minerd.exe",
                                       "-a cryptonight -o " + pool_address + " -u " + xmr_address + " -p x -t " + threads)
        oStartInfo.UseShellExecute = False
        oStartInfo.RedirectStandardOutput = True
        oStartInfo.CreateNoWindow = True
        oStartInfo.RedirectStandardError = True
        oProcess.StartInfo = oStartInfo
        Console.Write("Starting process" + ControlChars.CrLf)
        setTextBoxTxt("Starting process", TextBoxOutput)
        oProcess.Start()

        Using oStreamReader As System.IO.StreamReader = oProcess.StandardError
            While True
                Console.Write("Getting new line...")
                sOutput = oStreamReader.ReadLine()
                Console.Write(sOutput + ControlChars.CrLf)
                setTextBoxTxt(sOutput, TextBoxOutput)
                Try
                    splitOutput = sOutput.Split(New Char() {",", " ", "/"})
                    For Each part In splitOutput
                        Console.Write(cntr.ToString + " " + part + ControlChars.CrLf)
                        cntr += 1
                    Next
                    If sOutput.Contains("yay!") Then
                        Console.Write("yay!")
                        setTextBoxTxt(splitOutput(3), TextBoxShares)
                        setTextBoxTxt(splitOutput(4) - splitOutput(3), TextBoxBadShares)
                        setTextBoxTxt(splitOutput(7) + " h/s", TextBoxHashRate)
                    End If
                    'Console.Write(splitOutput.ToString + ControlChars.CrLf)
                Catch
                    Console.Write("null")
                End Try
                cntr = 0
            End While
        End Using
    End Sub

    Private Sub setTextBoxTxt(ByVal text As String, ByVal txtbox As TextBox)
        If txtbox.InvokeRequired Then
            txtbox.Invoke(New setLabelTxtInvoker(AddressOf setTextBoxTxt), text, txtbox)
        Else
            txtbox.Text = text
        End If
    End Sub
    Private Delegate Sub setLabelTxtInvoker(ByVal text As String, ByVal txtbox As TextBox)

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) _
        Handles Me.FormClosing
        Dim file As System.IO.StreamWriter
        file = My.Computer.FileSystem.OpenTextFileWriter("monerospelunker.conf", False)
        Dim pool_address = Me.TextBoxPoolAddress.Text()
        Dim myr_address = Me.TextBoxXmrAddress.Text()
        Dim threads = Me.TextBoxThreads.Text()
        file.WriteLine("Pool=" + pool_address)
        file.WriteLine("Address=" + myr_address)
        file.WriteLine("Threads=" + threads)
        Console.WriteLine("Pool=" + pool_address)
        Console.WriteLine("Address=" + myr_address)
        Console.WriteLine("Threads=" + threads)
        file.Close()
        Try
            oProcess.Kill()
        Catch
            Console.WriteLine("Already killed 1")
        End Try

        For Each p As Process In Process.GetProcesses
            Dim ListBox1 As New ListBox
            ListBox1.Items.Add(p.ProcessName.ToString)
            If String.Compare(p.ProcessName, "minerd.exe", True) = 0 Then
                p.Kill()
            End If
        Next
    End Sub

    Private Sub ButtonStop_Click(sender As Object, e As EventArgs) Handles ButtonStop.Click
        Try
            oProcess.Kill()
        Catch
            Console.WriteLine("Already killed 2")
        End Try
        Me.Close()
        Application.Exit()
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class
