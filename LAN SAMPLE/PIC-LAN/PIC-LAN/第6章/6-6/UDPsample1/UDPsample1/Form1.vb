'Socket�N���X�̌Ăяo��
Imports System
Imports System.Net
Imports System.Net.Sockets
Imports System.Text

Public Class Form1
    Inherits System.Windows.Forms.Form

    'Socket�N���X �C���X�^���X����
    Dim UdpClientA As New UdpClient()
    '��M�|�[�g��`�i�C�ӃA�h���X�j
    Dim Remote As New IPEndPoint(IPAddress.Any, 0)

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        '���胊���[�g�̐ڑ��i���O�ƃ|�[�g�ԍ��Ŏw��j
        Try
            UdpClientA.Connect(TextBox9.Text, 10002)
            TextBox1.Text = "Connect!"
        Catch ex As Exception
            TextBox1.Text = "Not Connected?"
        End Try
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Timer1.Enabled = False      '�^�C�}�P��~
        UdpClientA.Close()          'UDP�̃N���[�Y
        Close()
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        '�R�}���h�f�[�^���M
        Me.Enabled = False
        Try
            '���ݏ�Ԃɂ��I���I�t�𔽓]����
            If Button3.BackColor = Color.Red Then
                UdpClientA.Send(LED1Off, LED1Off.Length)
            Else
                UdpClientA.Send(LED1On, LED1On.Length)
            End If
            '�܂�Ԃ��f�[�^��M�iMLXXE)
            receiveBytes = UdpClientA.Receive(Remote)
            '�I���I�t��Ԃɂ��F�ƃe�L�X�g��ύX
            If receiveBytes(1) = &H4C And receiveBytes(2) = &H31 Then
                Button3.BackColor = Color.Red
            End If
            If receiveBytes(1) = &H4C And receiveBytes(2) = &H30 Then
                Button3.BackColor = Color.Green
            End If
        Catch ex As Exception
            TextBox1.Text = ex.ToString()
        End Try
        Me.Enabled = True
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        '�R�}���h�f�[�^���M
        Me.Enabled = False
        Try
            '���ݏ�Ԃɂ��I���I�t�𔽓]����
            If Button4.BackColor = Color.Red Then
                UdpClientA.Send(LED2Off, LED2Off.Length)
            Else
                UdpClientA.Send(LED2On, LED2On.Length)
            End If
            '�܂�Ԃ��f�[�^��M(MLXXE)
            receiveBytes = UdpClientA.Receive(Remote)
            '�I���I�t��Ԃɂ��F��ύX
            If receiveBytes(1) = &H4C And receiveBytes(3) = &H31 Then
                Button4.BackColor = Color.Red
            End If
            If receiveBytes(1) = &H4C And receiveBytes(3) = &H30 Then
                Button4.BackColor = Color.Green
            End If
        Catch ex As Exception
            TextBox1.Text = ex.ToString()
        End Try
        Me.Enabled = True
    End Sub


    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Timer1.Enabled = False
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        Me.Enabled = False
        Try
            '�X�C�b�`��ԓ��̓R�}���h���M
            UdpClientA.Send(SWInput, SWInput.Length)
            '�܂�Ԃ���M�iMBXXXX)
            receiveBytes = UdpClientA.Receive(Remote)
            'X��0��1�ŃI���I�t�\������
            If receiveBytes(2) = &H30 Then
                TextBox2.BackColor = Color.Red
            Else
                TextBox2.BackColor = Color.Green
            End If
            If receiveBytes(3) = &H30 Then
                TextBox3.BackColor = Color.Red
            Else
                TextBox3.BackColor = Color.Green
            End If
            If receiveBytes(4) = &H30 Then
                TextBox4.BackColor = Color.Red
            Else
                TextBox4.BackColor = Color.Green
            End If
            If receiveBytes(5) = &H30 Then
                TextBox5.BackColor = Color.Red
            Else
                TextBox5.BackColor = Color.Green
            End If
        Catch ex As Exception
            TextBox1.Text = ex.ToString()
        End Try
        Me.Enabled = True
    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        If Timer1.Enabled = False Then
            Timer1.Interval() = 500     '500msec�^�C�}�N��
            Timer1.Enabled() = True     '�^�C�}�P�N��
            Button6.Text = "��~"
        Else
            Timer1.Enabled = False
            Button6.Text = "�v��"

        End If
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Me.Enabled = False
        Try
            '�`���l��1�v��()
            UdpClientA.Send(AN2Mesure, AN2Mesure.Length)
            '�܂�Ԃ��f�[�^��M�iMAXXXX)
            receiveBytes = UdpClientA.Receive(Remote)
            '�f�[�^�ϊ��ƕ\��
            receiveBytes(0) = &H20              'M���X�y�[�X�ɕύX
            receiveBytes(1) = &H20              'A���X�y�[�X�ɕύX
            returnData = Encoding.ASCII.GetString(receiveBytes)
            TextBox6.Text = returnData.ToString()
            Application.DoEvents()              '�e�L�X�g�{�b�N�X�\�����\��
            '�`���l��2�v��
            UdpClientA.Send(AN3Mesure, AN3Mesure.Length)
            '�܂�Ԃ��f�[�^��M�iMAXXXX)
            receiveBytes = UdpClientA.Receive(Remote)
            '�v���f�[�^�ϊ��ƕ\��
            receiveBytes(0) = &H20              'M���X�y�[�X�ɕύX
            receiveBytes(1) = &H20              'A���X�y�[�X�ɕύX
            returnData = Encoding.ASCII.GetString(receiveBytes)
            TextBox7.Text = returnData.ToString()
            Application.DoEvents()              '�e�L�X�g�{�b�N�X�\�����\��
        Catch ex As Exception
            TextBox1.Text = ex.ToString()
        End Try
        Me.Enabled = True
    End Sub

    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
        Me.Enabled = False
        Try
            '�t���\����ւ̕\������
            Dim SendMessage As Byte()
            SendMessage = Encoding.ASCII.GetBytes("SD" + TextBox8.Text)
            UdpClientA.Send(SendMessage, SendMessage.Length)
            '�܂�Ԃ��f�[�^��M�iM?)
            receiveBytes = UdpClientA.Receive(Remote)
            '�f�[�^�ϊ��ƕ\��
            returnData = Encoding.ASCII.GetString(receiveBytes)
            TextBox1.Text = returnData.ToString()
        Catch ex As Exception
            TextBox1.Text = ex.ToString()
        End Try
        Me.Enabled = True
    End Sub

    Private Sub Button8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button8.Click
        Me.Enabled = False
        Try
            '�t���\����̏���
            UdpClientA.Send(LCDErase, LCDErase.Length)
            '�܂�Ԃ��f�[�^��M�iM?)
            receiveBytes = UdpClientA.Receive(Remote)
            '�f�[�^�ϊ��ƕ\��
            returnData = Encoding.ASCII.GetString(receiveBytes)
            TextBox1.Text = returnData.ToString()
        Catch ex As Exception
            TextBox1.Text = ex.ToString()
        End Try
        Me.Enabled = True
    End Sub

    '���ʕϐ��̒�`
    Dim receiveBytes As Byte()
    Dim returnData As String

    '���M�R�}���h�f�[�^��`
    Dim LED1On As Byte() = Encoding.ASCII.GetBytes("SC11")
    Dim LED1Off As Byte() = Encoding.ASCII.GetBytes("SC10")
    Dim LED2On As Byte() = Encoding.ASCII.GetBytes("SC21")
    Dim LED2Off As Byte() = Encoding.ASCII.GetBytes("SC20")
    Dim AN2Mesure As Byte() = Encoding.ASCII.GetBytes("SA1")
    Dim AN3Mesure As Byte() = Encoding.ASCII.GetBytes("SA2")
    Dim SWInput As Byte() = Encoding.ASCII.GetBytes("SB")
    Dim LCDErase As Byte() = Encoding.ASCII.GetBytes("SE")

End Class
