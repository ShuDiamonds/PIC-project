'Socket�N���X�̌Ăяo��
Imports System
Imports System.Net
Imports System.Net.Sockets
Imports System.Text

Public Class Form1
    'Socket�N���X �C���X�^���X����
    Dim UdpClientA As New UdpClient()
    '��M�|�[�g��`�i�C�ӃA�h���X�j
    Dim Remote As New IPEndPoint(IPAddress.Any, 0)
    '���ʕϐ��̒�`
    Dim receiveBytes(1000) As Byte
    Dim returnData As String
    Dim VoiceData(10000000) As Byte
    Dim Index As Long
    Dim MaxIndex As Long
    Dim EndFlag As Boolean

    '���M�R�}���h�f�[�^��`
    Dim TestMode As Byte() = Encoding.ASCII.GetBytes("ST")
    Dim OutputMode As Byte() = Encoding.ASCII.GetBytes("SO")
    Dim InputMode As Byte() = Encoding.ASCII.GetBytes("SI")
    Dim LoopMode As Byte() = Encoding.ASCII.GetBytes("SM")
    Dim EndMode As Byte() = Encoding.ASCII.GetBytes("SE")




    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        '���胊���[�g�̐ڑ��i���O�ƃ|�[�g�ԍ��Ŏw��j
        Try
            UdpClientA.Connect(TextBox1.Text, 10002)
            TextBox2.Text = "Connect!"
        Catch ex As Exception
            TextBox2.Text = "Not Connected?"
        End Try
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        UdpClientA.Close()          'UDP�̃N���[�Y
        Close()
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Me.Enabled = False
        Try
            '�e�X�g���[�h�̐ݒ萧��
            UdpClientA.Send(TestMode, TestMode.Length)
            '�܂�Ԃ��f�[�^��M�iMD1)
            receiveBytes = UdpClientA.Receive(Remote)
            '�f�[�^�ϊ��ƕ\��
            returnData = Encoding.ASCII.GetString(receiveBytes)
            TextBox2.Text = returnData.ToString()
        Catch ex As Exception
            TextBox2.Text = ex.ToString()
        End Try
        Me.Enabled = True
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Me.Enabled = False
        Try
            '�܂�Ԃ����[�h�̐ݒ萧��
            UdpClientA.Send(LoopMode, LoopMode.Length)
            '�܂�Ԃ��f�[�^��M�iMD1)
            receiveBytes = UdpClientA.Receive(Remote)
            '�f�[�^�ϊ��ƕ\��
            returnData = Encoding.ASCII.GetString(receiveBytes)
            TextBox2.Text = returnData.ToString()
        Catch ex As Exception
            TextBox2.Text = ex.ToString()
        End Try
        Me.Enabled = True
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        Dim i As Integer
        Me.Enabled = False
        Try
            '�����^�����[�h�̐ݒ萧��
            UdpClientA.Send(InputMode, InputMode.Length)
            Index = 0
            '�f�[�^�i�[
            While (receiveBytes(0) <> &H45 Or receiveBytes(1) <> &H4E) And (Index < 9000000)
                '�܂�Ԃ��f�[�^��M�iMD1)�@�Ō��0x00�͏���
                receiveBytes = UdpClientA.Receive(Remote)
                For i = 0 To receiveBytes.Length - 2
                    VoiceData(Index) = receiveBytes(i)
                    Index = Index + 1
                Next
            End While
            MaxIndex = Index
            TextBox2.Text = Index.ToString
        Catch ex As Exception
            TextBox2.Text = ex.ToString()
        End Try
        Me.Enabled = True
    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        Dim i As Integer
        Dim SendMessage(802) As Byte
        Button7.Focus()

        Try
            '�����Đ����[�h�̐ݒ萧��
            Index = 0
            EndFlag = False
            While ((Index < MaxIndex) And (EndFlag = False))
                SendMessage(0) = &H53               'S
                SendMessage(1) = &H4F               'O
                For i = 0 To 799
                    SendMessage(i + 2) = VoiceData(Index)
                    Index = Index + 1
                Next
                UdpClientA.Send(SendMessage, SendMessage.Length)
                '�܂�Ԃ��f�[�^��M�҂�
                receiveBytes = UdpClientA.Receive(Remote)
                Application.DoEvents()              '���̑���\�Ƃ���
            End While
            UdpClientA.Send(EndMode, EndMode.Length)
            TextBox2.Text = Index.ToString
        Catch ex As Exception
            TextBox2.Text = ex.ToString()
        End Try
    End Sub


    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
        EndFlag = True
        Index = 0
    End Sub

    Private Sub Button8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button8.Click
        '�t�@�C���ۑ�
        Dim SaveData(10000000) As Byte
        Dim N As Long

        SaveData = New Byte(MaxIndex) {}
        For N = 0 To MaxIndex
            SaveData(N) = VoiceData(N)
        Next
        '�t�@�C���_�C�A���O����t�@�C�����擾
        SaveFileDialog1.Filter = "data file(*.dat)|*dat|all file(*.*)|*.*"
        SaveFileDialog1.FilterIndex = 1
        If SaveFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            My.Computer.FileSystem.WriteAllBytes(SaveFileDialog1.FileName, SaveData, False)
            FileClose()
        End If
    End Sub

    Private Sub Button9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button9.Click
        '�t�@�C���ǂݏo��
        '�t�@�C���_�C�A���O����t�@�C�����擾
        OpenFileDialog1.Filter = "data file(*.dat)|*dat|all file(*.*)|*.*"
        OpenFileDialog1.FilterIndex = 1
        If OpenFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            '�t�@�C���ǂݏo�����s
            VoiceData = My.Computer.FileSystem.ReadAllBytes(OpenFileDialog1.FileName)
            MaxIndex = My.Computer.FileSystem.ReadAllBytes(OpenFileDialog1.FileName).Length - 800
            FileClose()
        End If
    End Sub
End Class
