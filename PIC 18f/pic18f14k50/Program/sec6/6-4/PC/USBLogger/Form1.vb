Public Class Form1
    Inherits System.Windows.Forms.Form

    Dim hComm As Integer                'COM�|�[�g�p�n���h�����O
    Dim stDCB As DCB                    'DCB�\���̒�`
    Dim timeOut As COMMTIMEOUTS         'COM�p�^�C���A�E�g�萔
    Dim wDATA As String                 'USB���M�o�b�t�@
    Dim rDATA(100) As Byte              'USB��M�o�b�t�@
    Dim dLen, wLen, rLen As Int32       'USB����M�p�p�����[�^
    Dim bRet As Boolean                 'USB�֐��߂�l
    Dim CommName As String              'COM�|�[�g�ԍ�
    Dim Flag As Integer                 '�f�[�^�ۑ��I���t���O
    Dim mDATA(10000) As Integer         '�f�[�^�ۑ��p�z��
    Dim Index As Integer                '�z��p�C���f�b�N�X


#Region " Windows �t�H�[�� �f�U�C�i�Ő������ꂽ�R�[�h "

    Public Sub New()
        MyBase.New()

        ' ���̌Ăяo���� Windows �t�H�[�� �f�U�C�i�ŕK�v�ł��B
        InitializeComponent()

        ' InitializeComponent() �Ăяo���̌�ɏ�������ǉ����܂��B

    End Sub

    ' Form �́A�R���|�[�l���g�ꗗ�Ɍ㏈�������s���邽�߂� dispose ���I�[�o�[���C�h���܂��B
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    ' Windows �t�H�[�� �f�U�C�i�ŕK�v�ł��B
    Private components As System.ComponentModel.IContainer

    ' ���� : �ȉ��̃v���V�[�W���́AWindows �t�H�[�� �f�U�C�i�ŕK�v�ł��B
    'Windows �t�H�[�� �f�U�C�i���g���ĕύX���Ă��������B  
    ' �R�[�h �G�f�B�^���g���ĕύX���Ȃ��ł��������B
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents TextBox3 As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents Button4 As System.Windows.Forms.Button
    Friend WithEvents Button5 As System.Windows.Forms.Button
    Friend WithEvents TextBox4 As System.Windows.Forms.TextBox
    Friend WithEvents Button6 As System.Windows.Forms.Button
    Friend WithEvents Button7 As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Button8 As System.Windows.Forms.Button
    Friend WithEvents TextBox5 As System.Windows.Forms.TextBox
    Friend WithEvents Button9 As System.Windows.Forms.Button
    Friend WithEvents Label4 As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.TextBox1 = New System.Windows.Forms.TextBox
        Me.Button1 = New System.Windows.Forms.Button
        Me.TextBox2 = New System.Windows.Forms.TextBox
        Me.Button2 = New System.Windows.Forms.Button
        Me.TextBox3 = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Button3 = New System.Windows.Forms.Button
        Me.Button4 = New System.Windows.Forms.Button
        Me.Button5 = New System.Windows.Forms.Button
        Me.TextBox4 = New System.Windows.Forms.TextBox
        Me.Button6 = New System.Windows.Forms.Button
        Me.Button7 = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Button8 = New System.Windows.Forms.Button
        Me.TextBox5 = New System.Windows.Forms.TextBox
        Me.Button9 = New System.Windows.Forms.Button
        Me.Label4 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'TextBox1
        '
        Me.TextBox1.Font = New System.Drawing.Font("�l�r �S�V�b�N", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.TextBox1.Location = New System.Drawing.Point(16, 16)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(48, 23)
        Me.TextBox1.TabIndex = 0
        Me.TextBox1.Text = "COM6"
        Me.TextBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.Color.SteelBlue
        Me.Button1.Font = New System.Drawing.Font("�l�r �S�V�b�N", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Button1.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.Button1.Location = New System.Drawing.Point(80, 8)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(88, 32)
        Me.Button1.TabIndex = 1
        Me.Button1.Text = "USB�ڑ�"
        '
        'TextBox2
        '
        Me.TextBox2.Font = New System.Drawing.Font("�l�r �S�V�b�N", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.TextBox2.Location = New System.Drawing.Point(192, 16)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(64, 23)
        Me.TextBox2.TabIndex = 2
        Me.TextBox2.Text = "Fault"
        Me.TextBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Button2
        '
        Me.Button2.BackColor = System.Drawing.Color.SteelBlue
        Me.Button2.Font = New System.Drawing.Font("�l�r �S�V�b�N", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Button2.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.Button2.Location = New System.Drawing.Point(352, 8)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(104, 32)
        Me.Button2.TabIndex = 3
        Me.Button2.Text = "�I��"
        '
        'TextBox3
        '
        Me.TextBox3.Font = New System.Drawing.Font("�l�r �S�V�b�N", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.TextBox3.Location = New System.Drawing.Point(16, 184)
        Me.TextBox3.Multiline = True
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.Size = New System.Drawing.Size(448, 72)
        Me.TextBox3.TabIndex = 4
        Me.TextBox3.Text = "Data"
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("�l�r �S�V�b�N", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(16, 120)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(144, 16)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "�v���f�[�^�\��/�ۑ�"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Button3
        '
        Me.Button3.BackColor = System.Drawing.Color.SteelBlue
        Me.Button3.Font = New System.Drawing.Font("�l�r �S�V�b�N", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Button3.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.Button3.Location = New System.Drawing.Point(16, 136)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(104, 32)
        Me.Button3.TabIndex = 6
        Me.Button3.Text = "�f�[�^�\��"
        '
        'Button4
        '
        Me.Button4.BackColor = System.Drawing.Color.SteelBlue
        Me.Button4.Font = New System.Drawing.Font("�l�r �S�V�b�N", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Button4.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.Button4.Location = New System.Drawing.Point(184, 136)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(104, 32)
        Me.Button4.TabIndex = 7
        Me.Button4.Text = "�f�[�^�ۑ�"
        '
        'Button5
        '
        Me.Button5.BackColor = System.Drawing.Color.SteelBlue
        Me.Button5.Font = New System.Drawing.Font("�l�r �S�V�b�N", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Button5.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.Button5.Location = New System.Drawing.Point(352, 136)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(104, 32)
        Me.Button5.TabIndex = 8
        Me.Button5.Text = "������"
        '
        'TextBox4
        '
        Me.TextBox4.Font = New System.Drawing.Font("�l�r �S�V�b�N", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.TextBox4.Location = New System.Drawing.Point(120, 80)
        Me.TextBox4.Name = "TextBox4"
        Me.TextBox4.Size = New System.Drawing.Size(32, 23)
        Me.TextBox4.TabIndex = 9
        Me.TextBox4.Text = "10"
        Me.TextBox4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Button6
        '
        Me.Button6.BackColor = System.Drawing.Color.SteelBlue
        Me.Button6.Font = New System.Drawing.Font("�l�r �S�V�b�N", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Button6.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.Button6.Location = New System.Drawing.Point(16, 72)
        Me.Button6.Name = "Button6"
        Me.Button6.Size = New System.Drawing.Size(96, 32)
        Me.Button6.TabIndex = 10
        Me.Button6.Text = "�����ݒ�"
        '
        'Button7
        '
        Me.Button7.BackColor = System.Drawing.Color.SteelBlue
        Me.Button7.Font = New System.Drawing.Font("�l�r �S�V�b�N", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Button7.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.Button7.Location = New System.Drawing.Point(184, 72)
        Me.Button7.Name = "Button7"
        Me.Button7.Size = New System.Drawing.Size(104, 32)
        Me.Button7.TabIndex = 11
        Me.Button7.Text = "����J�n"
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("�l�r �S�V�b�N", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(8, 56)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(120, 16)
        Me.Label2.TabIndex = 12
        Me.Label2.Text = "�f�[�^���O����"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("�l�r �S�V�b�N", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label3.Location = New System.Drawing.Point(16, 264)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(104, 16)
        Me.Label3.TabIndex = 13
        Me.Label3.Text = "���ڕ\������"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Button8
        '
        Me.Button8.BackColor = System.Drawing.Color.SteelBlue
        Me.Button8.Font = New System.Drawing.Font("�l�r �S�V�b�N", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Button8.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.Button8.Location = New System.Drawing.Point(16, 280)
        Me.Button8.Name = "Button8"
        Me.Button8.Size = New System.Drawing.Size(104, 32)
        Me.Button8.TabIndex = 14
        Me.Button8.Text = "�f�[�^�v��"
        '
        'TextBox5
        '
        Me.TextBox5.Font = New System.Drawing.Font("�l�r �S�V�b�N", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.TextBox5.Location = New System.Drawing.Point(152, 288)
        Me.TextBox5.Name = "TextBox5"
        Me.TextBox5.Size = New System.Drawing.Size(240, 23)
        Me.TextBox5.TabIndex = 15
        Me.TextBox5.Text = "Data"
        Me.TextBox5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Button9
        '
        Me.Button9.BackColor = System.Drawing.Color.FromArgb(CType(255, Byte), CType(192, Byte), CType(192, Byte))
        Me.Button9.Font = New System.Drawing.Font("MS UI Gothic", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Button9.Location = New System.Drawing.Point(352, 72)
        Me.Button9.Name = "Button9"
        Me.Button9.Size = New System.Drawing.Size(104, 32)
        Me.Button9.TabIndex = 16
        Me.Button9.Text = "EEPROM����"
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(376, 112)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(80, 16)
        Me.Label4.TabIndex = 17
        Me.Label4.Text = "���s���Ԓ���"
        '
        'Form1
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 12)
        Me.BackColor = System.Drawing.Color.LightSteelBlue
        Me.ClientSize = New System.Drawing.Size(480, 326)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Button9)
        Me.Controls.Add(Me.TextBox5)
        Me.Controls.Add(Me.Button8)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Button7)
        Me.Controls.Add(Me.Button6)
        Me.Controls.Add(Me.TextBox4)
        Me.Controls.Add(Me.Button5)
        Me.Controls.Add(Me.Button4)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.TextBox3)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.TextBox2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.TextBox1)
        Me.Name = "Form1"
        Me.Text = "Data Logger"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub TimeOutSet()
        'COM�ʐM�^�C���A�E�g�p�\���̃f�[�^�ݒ�
        timeOut.ReadIntervalTimeout = 10
        timeOut.ReadTotalTimeoutConstant = 30
        timeOut.ReadTotalTimeoutMultiplier = 10
        timeOut.WriteTotalTimeoutMultiplier = 10
        timeOut.WriteTotalTimeoutConstant = 30
        bRet = SetCommTimeouts(hComm, timeOut)
    End Sub


    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        'USB�ڑ�����
        Dim N As Integer
        CommName = TextBox1.Text            'COM�|�[�g�ԍ��擾
        'COM�|�[�g�I�[�v��
        bRet = CloseHandle(hComm)
        hComm = CreateFile(CommName, &HC0000000, 0, IntPtr.Zero, &H3, &H80, IntPtr.Zero)
        If hComm <> -1 Then
            'DCB�f�[�^�ݒ�
            stDCB.BaudRate = 115200
            stDCB.fBitFields = &H1
            stDCB.ByteSize = 8
            stDCB.Parity = 0
            stDCB.StopBits = 0
            bRet = SetCommState(hComm, stDCB)
            TimeOutSet()
            '�f�o�C�X�ڑ��m�F �R�}���h�u0�v�̉����uOK�v���`�F�b�N
            If bRet = True Then
                Application.DoEvents()
                wDATA = Chr(&H30)           '�R�}���h0�@OK���ԐM
                dLen = 1
                '�R�}���h���M��@�܂�Ԃ���M
                bRet = WriteFile(hComm, wDATA, dLen, wLen, IntPtr.Zero)
                bRet = ReadFile(hComm, rDATA, 30, rLen, IntPtr.Zero)
                If bRet = True And rLen = 3 Then
                    ' �ԐM�f�[�^�@"OK"�@���m�F
                    If rDATA(0) = &H4F And rDATA(1) = &H4B Then
                        TextBox2.Text = "Connect"      '����ڑ�����
                    Else
                        TextBox2.Text = "False"     '�ڑ��ُ�
                    End If
                Else
                    TextBox2.Text = "NoAns"             '�����Ȃ�
                End If
            Else
                TextBox2.Text = "Fault"             '�ڑ��ُ�
            End If
        Else
            TextBox2.Text = "NoExist"
        End If
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        '������
        TextBox2.Text = ""
        CloseHandle(0)
        Index = 0
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        '�I������
        Close()
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        '���O�f�[�^�ǂݎ��A�\������
        Dim N As Integer
        TextBox3.Text = ""
        bRet = SetCommState(hComm, stDCB)
        wDATA = Chr(&H33)           '�R�}���h
        dLen = Len(wDATA)
        '�R�}���h���M��@�܂�Ԃ�32�̃f�[�^�擾
        bRet = WriteFile(hComm, wDATA, dLen, wLen, IntPtr.Zero)
        bRet = ReadFile(hComm, rDATA, 64, rLen, IntPtr.Zero)
        If bRet = True Then
            '32�̃f�[�^��10�i���ŕ\��
            For N = 0 To rLen - 1 Step 2
                If (N > 16) And (N Mod 16 = 0) Then
                    TextBox3.Text = TextBox3.Text & vbCrLf
                End If
                TextBox3.Text = TextBox3.Text & Format(rDATA(N) * 256 + rDATA(N + 1), "00000") & "  "
            Next
        End If
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        '�f�[�^�ۑ�����
        Dim N As Integer
        Dim svFile1 As New SaveFileDialog
        '�����l�ݒ�
        TextBox3.Text = ""
        Index = 0
        Flag = False
        '�f�[�^�擾
        While Flag = False
            bRet = SetCommState(hComm, stDCB)
            wDATA = Chr(&H33)       '�R�}���h
            dLen = Len(wDATA)
            '�R�}���h���M��@64�o�C�g�̃f�[�^�擾
            bRet = WriteFile(hComm, wDATA, dLen, wLen, IntPtr.Zero)
            bRet = ReadFile(hComm, rDATA, 64, rLen, IntPtr.Zero)
            If bRet = True Then
                '�f�[�^���Q�o�C�g���ɔz��Ƀo�C�i���ŕۑ�
                For N = 0 To rLen - 1 Step 2
                    mDATA(Index) = rDATA(N) * 256 + rDATA(N + 1)
                    If mDATA(Index) = &HFFFF Then
                        Flag = True
                        Exit For
                    End If
                    Index = Index + 1
                Next
            End If
        End While
        '�擾�����t�@�C���ۑ�
        TextBox3.Text = "�f�[�^�擾����"
        bRet = MessageBox.Show("�t�@�C���ɕۑ����܂��B")
        '�t�@�C���_�C�A���O����t�@�C�����擾
        svFile1.Filter = "data file(*.dat)|*dat|all file(*.*)|*.*"
        svFile1.FilterIndex = 1
        svFile1.RestoreDirectory = True
        Dim ret As DialogResult = svFile1.ShowDialog
        If ret <> DialogResult.Cancel Then
            '�t�@�C���ۑ����s
            FileOpen(1, svFile1.FileName, OpenMode.Output)
            For N = 0 To Index
                Write(1, mDATA(N))
            Next
            FileClose(1)
        End If
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        'EEPROM�A�h���X������
        TextBox3.Text = ""
        bRet = SetCommState(hComm, stDCB)
        wDATA = Chr(&H30)       '�R�}���h
        dLen = Len(wDATA)
        '�R�}���h���M��܂�Ԃ��f�[�^�m�F
        bRet = WriteFile(hComm, wDATA, dLen, wLen, IntPtr.Zero)
        bRet = ReadFile(hComm, rDATA, 64, rLen, IntPtr.Zero)
        If bRet = True And rLen > 1 Then
            '���������튮��
            TextBox3.Text = "����������"
            Button7.Text = "����J�n"
        Else
            '�������ُ�I��
            TextBox3.Text = "���������s"
        End If
    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        '���O�����ݒ�
        bRet = SetCommState(hComm, stDCB)
        wDATA = Chr(&H31) & Chr(Val(TextBox4.Text))     '�����f�[�^�擾
        dLen = Len(wDATA)
        '�R�}���h���M
        bRet = WriteFile(hComm, wDATA, dLen, wLen, IntPtr.Zero)
    End Sub

    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
        '�f�[�^���O�J�n�A��~����
        bRet = SetCommState(hComm, stDCB)
        wDATA = Chr(&H37)           '�R�}���h
        dLen = Len(wDATA)
        bRet = WriteFile(hComm, wDATA, dLen, wLen, IntPtr.Zero)
        '��ԍX�V�A�J�n�A��~���ݐ���
        If Button7.Text = "�����~" Then
            Button7.Text = "����J�n"
        Else
            Button7.Text = "�����~"
        End If
    End Sub

    Private Sub Button8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button8.Click
        '�ʌv���v��
        Dim N As Integer
        TextBox5.Text = ""
        bRet = SetCommState(hComm, stDCB)
        wDATA = Chr(&H32)       '�R�}���h
        dLen = Len(wDATA)
        '�R�}���h���M��@�܂�Ԃ��̌v���f�[�^��M
        bRet = WriteFile(hComm, wDATA, dLen, wLen, IntPtr.Zero)
        bRet = ReadFile(hComm, rDATA, 64, rLen, IntPtr.Zero)
        If bRet = True Then
            '�v���f�[�^�\���@�R�`���l������
            For N = 0 To rLen - 1
                TextBox5.Text = TextBox5.Text & Chr(rDATA(N))
            Next
        End If
    End Sub

    Private Sub Button9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button9.Click
        'EEPROM����
        bRet = SetCommState(hComm, stDCB)
        wDATA = Chr(&H36)
        dLen = Len(wDATA)
        '�R�}���h���M
        bRet = WriteFile(hComm, wDATA, dLen, wLen, IntPtr.Zero)
        If bRet = True Then
            Close()
        End If
    End Sub
End Class
