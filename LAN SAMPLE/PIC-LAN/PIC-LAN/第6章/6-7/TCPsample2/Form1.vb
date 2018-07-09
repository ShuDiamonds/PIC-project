'Socket�N���X�̌Ăяo��
Imports System
Imports System.Net
Imports System.Net.Sockets
Imports System.Text

Public Class Form1
    'Socket�N���X �C���X�^���X����
    Dim TcpClientA As New TcpClient()
    Dim MyStream As NetworkStream

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
        Timer1.Enabled = False      '�^�C�}�P��~
        MyStream.Close()
        TcpClientA.Close()          'TCP�̃N���[�Y
        Close()
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Timer1.Enabled = False
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        '�R�}���h�f�[�^���M
        Me.Enabled = False
        Try
            '���ݏ�Ԃɂ��I���I�t�𔽓]����
            If Button3.BackColor = Color.Red Then
                MyStream.Write(LED1Off, 0, LED1Off.Length)
            Else
                MyStream.Write(LED1On, 0, LED1On.Length)
            End If
            '�܂�Ԃ��f�[�^��M�iMLXXE)
            MyStream.Read(RcvBuf, 0, RcvBuf.Length)
            '�I���I�t��Ԃɂ��F�ƃe�L�X�g��ύX
            If RcvBuf(1) = &H4C And RcvBuf(2) = &H31 Then
                Button3.BackColor = Color.Red
            End If
            If RcvBuf(1) = &H4C And RcvBuf(2) = &H30 Then
                Button3.BackColor = Color.Green
            End If
        Catch ex As Exception
            TextBox3.Text = ex.ToString()
        End Try
        Me.Enabled = True
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        '�R�}���h�f�[�^���M
        Me.Enabled = False
        Try
            '���ݏ�Ԃɂ��I���I�t�𔽓]����
            If Button4.BackColor = Color.Red Then
                MyStream.Write(LED2Off, 0, LED2Off.Length)
            Else
                MyStream.Write(LED2On, 0, LED2On.Length)
            End If

            '�܂�Ԃ��f�[�^��M�iMLXXE)
            MyStream.Read(RcvBuf, 0, RcvBuf.Length)
            '�I���I�t��Ԃɂ��F�ƃe�L�X�g��ύX
            If RcvBuf(1) = &H4C And RcvBuf(3) = &H31 Then
                Button4.BackColor = Color.Red
            End If
            If RcvBuf(1) = &H4C And RcvBuf(3) = &H30 Then
                Button4.BackColor = Color.Green
            End If
        Catch ex As Exception
            TextBox3.Text = ex.ToString()
        End Try
        Me.Enabled = True
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        Me.Enabled = False
        Try
            '�X�C�b�`��ԓ��̓R�}���h���M
            MyStream.Write(SWInput, 0, SWInput.Length)

            '�܂�Ԃ���M�iMBXXXX)
            MyStream.Read(RcvBuf, 0, RcvBuf.Length)
            'X��0��1�ŃI���I�t�\������
            If RcvBuf(2) = &H30 Then
                TextBox4.BackColor = Color.Red
            Else
                TextBox4.BackColor = Color.Green
            End If
            If RcvBuf(3) = &H30 Then
                TextBox5.BackColor = Color.Red
            Else
                TextBox5.BackColor = Color.Green
            End If
            If RcvBuf(4) = &H30 Then
                TextBox6.BackColor = Color.Red
            Else
                TextBox6.BackColor = Color.Green
            End If
            If RcvBuf(5) = &H30 Then
                TextBox7.BackColor = Color.Red
            Else
                TextBox7.BackColor = Color.Green
            End If
        Catch ex As Exception
            TextBox3.Text = ex.ToString()
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
            '�`���l��1�v��
            MyStream.Write(AN2Mesure, 0, AN2Mesure.Length)
            '�܂�Ԃ��f�[�^��M�iMAXXXX)
            MyStream.Read(RcvBuf, 0, RcvBuf.Length)
            '�f�[�^�ϊ��ƕ\��
            RcvBuf(0) = &H20              'M���X�y�[�X�ɕύX
            RcvBuf(1) = &H20              'A���X�y�[�X�ɕύX
            returnData = Encoding.ASCII.GetString(RcvBuf)
            TextBox8.Text = returnData.ToString()
            Application.DoEvents()        '�e�L�X�g�{�b�N�X�\���\��
            '�`���l��2�v��
            MyStream.Write(AN3Mesure, 0, AN3Mesure.Length)
            '�܂�Ԃ��f�[�^��M�iMAXXXX)
            MyStream.Read(RcvBuf, 0, RcvBuf.Length)
            '�v���f�[�^�ϊ��ƕ\��
            RcvBuf(0) = &H20              'M���X�y�[�X�ɕύX
            RcvBuf(1) = &H20              'A���X�y�[�X�ɕύX
            returnData = Encoding.ASCII.GetString(RcvBuf)
            TextBox9.Text = returnData.ToString()
            Application.DoEvents()        '�e�L�X�g�{�b�N�X�\���\��
        Catch ex As Exception
            TextBox3.Text = ex.ToString()
        End Try
        Me.Enabled = True
    End Sub

    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
        Me.Enabled = False
        Try
            '�t���\����ւ̕\������
            Dim SendMessage As Byte()

            SendMessage = Encoding.ASCII.GetBytes("SD" + TextBox10.Text + vbNullChar)
            MyStream.Write(SendMessage, 0, SendMessage.Length)
            '�܂�Ԃ��f�[�^��M�iM?)
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
            '�t���\����̏���
            MyStream.Write(LCDErase, 0, LCDErase.Length)
            '�܂�Ԃ��f�[�^��M�iM?)
            MyStream.Read(RcvBuf, 0, RcvBuf.Length)
            '�f�[�^�ϊ��ƕ\��
            returnData = Encoding.ASCII.GetString(RcvBuf)
            TextBox3.Text = returnData.ToString()
        Catch ex As Exception
            TextBox3.Text = ex.ToString()
        End Try
        Me.Enabled = True
    End Sub

    '���ʕϐ��̒�`
    Dim receiveBytes As Byte()
    Dim returnData As String
    Dim RcvBuf(200) As Byte
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
