Public Class ReceiveFrom

    Dim udpReceiver As UDPReceiver

    Private Sub btnStart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStart.Click
        Dim port As Integer
        If (Integer.TryParse(txtPort.Text, port)) Then
            udpReceiver = New UDPReceiver(port)
            AddHandler udpReceiver.UpdatePinStatus, AddressOf udpReceiver_UpdatePinStatus
            udpReceiver.Start()
            txtLog.Text += "UDP command receiver is active on port: " & port.ToString() & vbCrLf
        Else
            MessageBox.Show("Please Enter Port Number")
        End If

        btnStart.Enabled = False
        btnStop.Enabled = True
    End Sub

    Public Sub udpReceiver_UpdatePinStatus(ByVal sender As Object, ByVal e As UpdatePinStatusArgs)
        UpdateLogTxtBox(e)
        UpdateStatusButton(e)
    End Sub

    Delegate Sub SetTextCallback(ByVal pinStatus As UpdatePinStatusArgs)

    'Perform thread safe txtLog update
    Private Sub UpdateLogTxtBox(ByVal pinStatus As UpdatePinStatusArgs)
        If Me.txtLog.InvokeRequired Then
            Dim d As New SetTextCallback(AddressOf UpdateLogTxtBox)
            Me.Invoke(d, New Object() {pinStatus})
        Else
            Me.txtLog.Text += pinStatus.ToString()
        End If
    End Sub

    Delegate Sub SetButtonCallback(ByVal pinStatus As UpdatePinStatusArgs)
    Private btn As New Button

    'Perform thread safe status button update
    Public Sub UpdateStatusButton(ByVal pinStatus As UpdatePinStatusArgs)
        btn = CType(Me.Controls.Find("btnStatus" + pinStatus.Pin.ToString(), True)(0), Button)
        If Me.btn.InvokeRequired Then
            Dim d As New SetButtonCallback(AddressOf UpdateStatusButton)
            Me.Invoke(d, New Object() {pinStatus})
        Else
            If pinStatus.Status = 0 Then
                Me.btn.BackColor = Color.Firebrick
                Me.btn.Text = "Off"
            ElseIf pinStatus.Status = 1 Then
                Me.btn.BackColor = Color.Green
                Me.btn.Text = "On"
            Else
                Me.BackColor = Color.Orange
                Me.btn.Text = "Error"
            End If
        End If
    End Sub

    Private Sub btnStop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStop.Click
        btnStart.Enabled = True
        btnStop.Enabled = False
        txtLog.Clear()
        RemoveHandler udpReceiver.UpdatePinStatus, AddressOf udpReceiver_UpdatePinStatus
        udpReceiver.Close()
    End Sub

End Class
