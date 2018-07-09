'Socket�N���X�̌Ăяo��
Imports System
Imports System.Net
Imports System.Net.Sockets
Imports System.Text

Public Class Form1
    'Socket�N���X �C���X�^���X����
    Dim TcpClientA As New TcpClient()
    Dim MyStream As NetworkStream
    '���ʕϐ��̒�`
    Dim receiveBytes As Byte()
    Dim returnData As String
    Dim RcvBuf(100) As Byte

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        'TCP�����[�g�ڑ�
        Try
            TcpClientA.Connect(TextBox1.Text, Integer.Parse(TextBox2.Text))
            TextBox3.Text = "Connect!"
            MyStream = TcpClientA.GetStream()
        Catch ex As Exception
            TextBox3.Text = ex.ToString()
        End Try
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        MyStream.Close()
        TcpClientA.Close()          'TCP�̃N���[�Y
        Close()
    End Sub


    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Me.Enabled = False
        Try
            '�T�[�o�A�h���X�ݒ�
            Dim SendMessage As Byte()

            SendMessage = Encoding.ASCII.GetBytes("SA" + TextBox4.Text + vbNullChar)
            MyStream.Write(SendMessage, 0, SendMessage.Length)
            '�܂�Ԃ��f�[�^��M
            MyStream.Read(RcvBuf, 0, RcvBuf.Length)
            '�f�[�^�ϊ��ƕ\��
            returnData = Encoding.ASCII.GetString(RcvBuf)
            TextBox3.Text = returnData.ToString()
        Catch ex As Exception
            TextBox3.Text = ex.ToString()
        End Try
        Me.Enabled = True
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Me.Enabled = False
        Try
            '����A�h���X
            Dim SendMessage As Byte()

            SendMessage = Encoding.ASCII.GetBytes("SB" + TextBox5.Text + vbNullChar)
            MyStream.Write(SendMessage, 0, SendMessage.Length)
            '�܂�Ԃ��f�[�^��M
            MyStream.Read(RcvBuf, 0, RcvBuf.Length)
            '�f�[�^�ϊ��ƕ\��
            returnData = Encoding.ASCII.GetString(RcvBuf)
            TextBox3.Text = returnData.ToString()
        Catch ex As Exception
            TextBox3.Text = ex.ToString()
        End Try
        Me.Enabled = True
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        Me.Enabled = False
        Try
            '�T�u�W�F�N�g
            Dim SendMessage As Byte()

            SendMessage = Encoding.Default.GetBytes("SC" + TextBox6.Text + vbNullChar)
            MyStream.Write(SendMessage, 0, SendMessage.Length)
            '�܂�Ԃ��f�[�^��M
            MyStream.Read(RcvBuf, 0, RcvBuf.Length)
            '�f�[�^�ϊ��ƕ\��
            returnData = Encoding.ASCII.GetString(RcvBuf)
            TextBox3.Text = returnData.ToString()
        Catch ex As Exception
            TextBox3.Text = ex.ToString()
        End Try
        Me.Enabled = True
    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        Me.Enabled = False
        Try
            '�x��P
            Dim SendMessage As Byte()

            SendMessage = Encoding.Default.GetBytes("SD" + TextBox7.Text + vbNullChar)
            MyStream.Write(SendMessage, 0, SendMessage.Length)
            '�܂�Ԃ��f�[�^��M
            MyStream.Read(RcvBuf, 0, RcvBuf.Length)
            '�f�[�^�ϊ��ƕ\��
            returnData = Encoding.ASCII.GetString(RcvBuf)
            TextBox3.Text = returnData.ToString()
        Catch ex As Exception
            TextBox3.Text = ex.ToString()
        End Try
        Me.Enabled = True
    End Sub

    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
        Me.Enabled = False
        Try
            '�x��Q
            Dim SendMessage As Byte()

            SendMessage = Encoding.Default.GetBytes("SE" + TextBox8.Text + vbNullChar)
            MyStream.Write(SendMessage, 0, SendMessage.Length)
            '�܂�Ԃ��f�[�^��M
            MyStream.Read(RcvBuf, 0, RcvBuf.Length)
            '�f�[�^�ϊ��ƕ\��
            returnData = Encoding.ASCII.GetString(RcvBuf)
            TextBox3.Text = returnData.ToString()
        Catch ex As Exception
            TextBox3.Text = ex.ToString()
        End Try
        Me.Enabled = True
    End Sub

    Private Sub Button8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button8.Click
        Me.Enabled = False
        Try
            '�x��3
            Dim SendMessage As Byte()

            SendMessage = Encoding.Default.GetBytes("SF" + TextBox9.Text + vbNullChar)
            MyStream.Write(SendMessage, 0, SendMessage.Length)
            '�܂�Ԃ��f�[�^��M
            MyStream.Read(RcvBuf, 0, RcvBuf.Length)
            '�f�[�^�ϊ��ƕ\��
            returnData = Encoding.ASCII.GetString(RcvBuf)
            TextBox3.Text = returnData.ToString()
        Catch ex As Exception
            TextBox3.Text = ex.ToString()
        End Try
        Me.Enabled = True
    End Sub

    Private Sub Button9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button9.Click
        Me.Enabled = False
        Try
            '�x��4
            Dim SendMessage As Byte()
            SendMessage = Encoding.Default.GetBytes("SG" + TextBox10.Text + vbNullChar)
            MyStream.Write(SendMessage, 0, SendMessage.Length)
            '�܂�Ԃ��f�[�^��M
            MyStream.Read(RcvBuf, 0, RcvBuf.Length)
            '�f�[�^�ϊ��ƕ\��
            returnData = Encoding.ASCII.GetString(RcvBuf)
            TextBox3.Text = returnData.ToString()
        Catch ex As Exception
            TextBox3.Text = ex.ToString()
        End Try
        Me.Enabled = True
    End Sub

    Private Sub Button10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button10.Click
        Me.Enabled = False
        Try
            '�x��4
            Dim SendMessage As Byte()
            SendMessage = Encoding.Default.GetBytes("SH" + TextBox11.Text + vbNullChar)
            MyStream.Write(SendMessage, 0, SendMessage.Length)
            '�܂�Ԃ��f�[�^��M
            MyStream.Read(RcvBuf, 0, RcvBuf.Length)
            '�f�[�^�ϊ��ƕ\��
            returnData = Encoding.ASCII.GetString(RcvBuf)
            TextBox3.Text = returnData.ToString()
        Catch ex As Exception
            TextBox3.Text = ex.ToString()
        End Try
        Me.Enabled = True
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub
End Class
