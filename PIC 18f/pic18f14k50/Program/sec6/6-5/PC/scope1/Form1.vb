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
    Dim Flag As Integer                 '��M�f�[�^�o�b�t�@�p�t���O
    Dim Threshold As Integer            '�X���V�����h�d���ݒ�
    Dim Channel As Integer              '�`���l���ԍ��ݒ�
    Dim FreeFlag As Integer             '�t���[�������[�h�t���O

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
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents Button4 As System.Windows.Forms.Button
    Friend WithEvents HScrollBar1 As System.Windows.Forms.HScrollBar
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Timer2 As System.Windows.Forms.Timer
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents HScrollBar2 As System.Windows.Forms.HScrollBar
    Friend WithEvents HScrollBar3 As System.Windows.Forms.HScrollBar
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Button5 As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents RadioButton1 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton2 As System.Windows.Forms.RadioButton
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.TextBox1 = New System.Windows.Forms.TextBox
        Me.Button1 = New System.Windows.Forms.Button
        Me.TextBox2 = New System.Windows.Forms.TextBox
        Me.Button2 = New System.Windows.Forms.Button
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.Button3 = New System.Windows.Forms.Button
        Me.Button4 = New System.Windows.Forms.Button
        Me.HScrollBar1 = New System.Windows.Forms.HScrollBar
        Me.Label1 = New System.Windows.Forms.Label
        Me.Timer2 = New System.Windows.Forms.Timer(Me.components)
        Me.Label3 = New System.Windows.Forms.Label
        Me.HScrollBar2 = New System.Windows.Forms.HScrollBar
        Me.HScrollBar3 = New System.Windows.Forms.HScrollBar
        Me.Label2 = New System.Windows.Forms.Label
        Me.Button5 = New System.Windows.Forms.Button
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.RadioButton2 = New System.Windows.Forms.RadioButton
        Me.RadioButton1 = New System.Windows.Forms.RadioButton
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TextBox1
        '
        Me.TextBox1.Font = New System.Drawing.Font("�l�r �S�V�b�N", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.TextBox1.Location = New System.Drawing.Point(32, 16)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(48, 23)
        Me.TextBox1.TabIndex = 0
        Me.TextBox1.Text = "COM9"
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(192, Byte), CType(192, Byte))
        Me.Button1.Font = New System.Drawing.Font("�l�r �S�V�b�N", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Button1.Location = New System.Drawing.Point(88, 8)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(112, 40)
        Me.Button1.TabIndex = 1
        Me.Button1.Text = "USB�ڑ�"
        '
        'TextBox2
        '
        Me.TextBox2.Font = New System.Drawing.Font("�l�r �S�V�b�N", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.TextBox2.Location = New System.Drawing.Point(208, 16)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(72, 23)
        Me.TextBox2.TabIndex = 2
        Me.TextBox2.Text = ""
        '
        'Button2
        '
        Me.Button2.BackColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(192, Byte), CType(192, Byte))
        Me.Button2.Font = New System.Drawing.Font("�l�r �S�V�b�N", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Button2.Location = New System.Drawing.Point(32, 64)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(112, 40)
        Me.Button2.TabIndex = 3
        Me.Button2.Text = "�蓮�v��"
        '
        'Timer1
        '
        '
        'Button3
        '
        Me.Button3.BackColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(192, Byte), CType(192, Byte))
        Me.Button3.Font = New System.Drawing.Font("�l�r �S�V�b�N", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Button3.Location = New System.Drawing.Point(312, 8)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(112, 40)
        Me.Button3.TabIndex = 4
        Me.Button3.Text = "�I��"
        '
        'Button4
        '
        Me.Button4.BackColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(192, Byte), CType(192, Byte))
        Me.Button4.Font = New System.Drawing.Font("�l�r �S�V�b�N", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Button4.Location = New System.Drawing.Point(168, 64)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(112, 40)
        Me.Button4.TabIndex = 5
        Me.Button4.Text = "�����v��"
        '
        'HScrollBar1
        '
        Me.HScrollBar1.Location = New System.Drawing.Point(120, 120)
        Me.HScrollBar1.Name = "HScrollBar1"
        Me.HScrollBar1.Size = New System.Drawing.Size(160, 24)
        Me.HScrollBar1.TabIndex = 6
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("�l�r �S�V�b�N", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label1.Location = New System.Drawing.Point(24, 120)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(88, 24)
        Me.Label1.TabIndex = 8
        Me.Label1.Text = "����������"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Timer2
        '
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("�l�r �S�V�b�N", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label3.Location = New System.Drawing.Point(16, 160)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(96, 24)
        Me.Label3.TabIndex = 11
        Me.Label3.Text = "�������x��"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'HScrollBar2
        '
        Me.HScrollBar2.Location = New System.Drawing.Point(120, 160)
        Me.HScrollBar2.Name = "HScrollBar2"
        Me.HScrollBar2.Size = New System.Drawing.Size(160, 24)
        Me.HScrollBar2.TabIndex = 12
        '
        'HScrollBar3
        '
        Me.HScrollBar3.Location = New System.Drawing.Point(120, 200)
        Me.HScrollBar3.Name = "HScrollBar3"
        Me.HScrollBar3.Size = New System.Drawing.Size(160, 24)
        Me.HScrollBar3.TabIndex = 13
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label2.Location = New System.Drawing.Point(16, 200)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(96, 24)
        Me.Label2.TabIndex = 14
        Me.Label2.Text = "�T���v������"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Button5
        '
        Me.Button5.BackColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(192, Byte), CType(192, Byte))
        Me.Button5.Font = New System.Drawing.Font("�l�r �S�V�b�N", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Button5.ForeColor = System.Drawing.SystemColors.ActiveCaption
        Me.Button5.Location = New System.Drawing.Point(312, 64)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(112, 40)
        Me.Button5.TabIndex = 15
        Me.Button5.Text = "�t���[����"
        '
        'GroupBox1
        '
        Me.GroupBox1.BackColor = System.Drawing.Color.Gainsboro
        Me.GroupBox1.Controls.Add(Me.RadioButton2)
        Me.GroupBox1.Controls.Add(Me.RadioButton1)
        Me.GroupBox1.Font = New System.Drawing.Font("�l�r �S�V�b�N", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(312, 128)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(112, 96)
        Me.GroupBox1.TabIndex = 16
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "�`���l���w��"
        '
        'RadioButton2
        '
        Me.RadioButton2.Font = New System.Drawing.Font("�l�r �S�V�b�N", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.RadioButton2.Location = New System.Drawing.Point(16, 56)
        Me.RadioButton2.Name = "RadioButton2"
        Me.RadioButton2.Size = New System.Drawing.Size(88, 24)
        Me.RadioButton2.TabIndex = 1
        Me.RadioButton2.Text = "CH1(AC)"
        '
        'RadioButton1
        '
        Me.RadioButton1.Checked = True
        Me.RadioButton1.Font = New System.Drawing.Font("�l�r �S�V�b�N", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.RadioButton1.Location = New System.Drawing.Point(16, 24)
        Me.RadioButton1.Name = "RadioButton1"
        Me.RadioButton1.Size = New System.Drawing.Size(88, 24)
        Me.RadioButton1.TabIndex = 0
        Me.RadioButton1.TabStop = True
        Me.RadioButton1.Text = "CH0(DC)"
        '
        'Form1
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 12)
        Me.BackColor = System.Drawing.Color.FromArgb(CType(192, Byte), CType(255, Byte), CType(255, Byte))
        Me.ClientSize = New System.Drawing.Size(448, 246)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Button5)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.HScrollBar3)
        Me.Controls.Add(Me.HScrollBar2)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.HScrollBar1)
        Me.Controls.Add(Me.Button4)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.TextBox2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.TextBox1)
        Me.Name = "Form1"
        Me.Text = "�I�V���X�R�[�v"
        Me.GroupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub TimeOutSet()
        'COM�|�[�g�p�^�C���A�E�g�֐�
        timeOut.ReadIntervalTimeout = 10
        timeOut.ReadTotalTimeoutConstant = 30
        timeOut.ReadTotalTimeoutMultiplier = 10
        timeOut.WriteTotalTimeoutMultiplier = 10
        timeOut.WriteTotalTimeoutConstant = 30
        bRet = SetCommTimeouts(hComm, timeOut)
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        F1 = Me                 '�t�H�[���̖��O�ݒ�
        '�\���s�b�`
        HScrollBar1.Minimum = 1
        HScrollBar1.Maximum = 10
        HScrollBar1.SmallChange = 1
        HScrollBar1.LargeChange = 1
        HScrollBar1.Value = 1
        HDIV = 1
        '�����X���V�����h
        HScrollBar2.Minimum = 1
        HScrollBar2.Maximum = 255
        HScrollBar2.SmallChange = 1
        HScrollBar2.LargeChange = 10
        Threshold = 90
        HScrollBar2.Value() = 90
        '�T���v�����O����
        HScrollBar3.Minimum = 1
        HScrollBar3.Maximum = 100
        HScrollBar3.SmallChange = 1
        HScrollBar3.LargeChange = 10
        HScrollBar3.Value = 5
        Sample = 5
        '�`���l���ԍ��w��
        If RadioButton1.Checked = True Then Channel = 0
        If RadioButton1.Checked = True Then Channel = 1
        TextBox2.Text = ""
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        'USB�ڑ����s
        Dim N As Integer
        CommName = TextBox1.Text              'COM�|�[�g�ԍ��擾
        '        bRet = CloseHandle(hComm)
        'COM�|�[�g�I�[�v��
        hComm = CreateFile(CommName, &HC0000000, 0, IntPtr.Zero, &H3, &H80, IntPtr.Zero)
        If hComm <> -1 Then
            '����ڑ����������ꍇ
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
                        F2.Show()
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

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        '����J�n
        Index = 0
        Timer2.Stop()
        Application.DoEvents()
        '����J�n�R�}���h
        bRet = SetCommState(hComm, stDCB)
        wDATA = Chr(&H31) + Chr(Threshold) + Chr(Sample) + Chr(Channel)
        dLen = Len(wDATA)
        '�R�}���h���M
        bRet = WriteFile(hComm, wDATA, dLen, wLen, IntPtr.Zero)

        '�J��Ԃ��v���������^�C�}�ɐݒ�ƊJ�n
        Application.DoEvents()
        Timer1.Interval() = 500
        Timer1.Start()
    End Sub


    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        '�v�������₢���킹�ƌJ��Ԃ�����
        Dim N, K As Integer
        Application.DoEvents()
        bRet = SetCommState(hComm, stDCB)
        wDATA = Chr(&H32)                           '�����₢���킹�R�}���h
        dLen = Len(wDATA)
        '�R�}���h���M�A�܂�Ԃ��f�[�^��M
        bRet = WriteFile(hComm, wDATA, dLen, wLen, IntPtr.Zero)
        bRet = ReadFile(hComm, rDATA, 64, rLen, IntPtr.Zero)
        If bRet = True Then
            '�f�[�^���W�������m�F
            If rDATA(0) = &H31 Then                 '�P�Ȃ犮��
                Timer1.Stop()                       '�₢���킹�p�^�C�}��~
                For K = 0 To 14 Step 1              '15�u���b�N�f�[�^���W
                    wDATA = Chr(&H33)               '�v���f�[�^�擾�R�}���h
                    dLen = Len(wDATA)
                    '�R�}���h���M�A�܂�Ԃ��f�[�^��M
                    bRet = WriteFile(hComm, wDATA, dLen, wLen, IntPtr.Zero)
                    bRet = ReadFile(hComm, rDATA, 64, rLen, IntPtr.Zero)
                    If bRet = True Then
                        '64�o�C�g�̃f�[�^���o�b�t�@�ۑ�
                        For N = 0 To rLen - 1 Step 1
                            mDATA(Index) = rDATA(N)
                            Index = Index + 1       '�C���f�b�N�X�͘A��
                        Next
                    End If
                Next K
                '�O���t�\���w���@�ʃt�H�[���I�[�v��
                F2.Activate()
            End If
        End If
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        '�I������
        Timer1.Stop()
        Timer2.Stop()
        CloseHandle(0)
        F2.Close()
        Close()
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        '�����v���@�^�C�}�Q�ŌJ��Ԃ�
        Application.DoEvents()
        FreeFlag = &H31
        Timer2.Interval = 2000
        Timer2.Start()
        Timer2.Enabled = True
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        '�t���[�������[�h����J�n()
        Application.DoEvents()
        FreeFlag = &H34
        Timer2.Interval = 2000
        Timer2.Start()
        Timer2.Enabled = True
    End Sub

    Private Sub Timer2_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer2.Tick
        '�����v���̏ꍇ�̌J��Ԃ�����
        Index = 0
        F1.Activate()                                           '�O���t�ĕ\���̂���F1�Ƀt�H�[�J�X
        Application.DoEvents()
        If RadioButton1.Checked = True Then Channel = 0
        If RadioButton2.Checked = True Then Channel = 1
        '����J�n�R�}���h
        bRet = SetCommState(hComm, stDCB)
        wDATA = Chr(FreeFlag) + Chr(Threshold) + Chr(Sample) + Chr(Channel)
        dLen = Len(wDATA)                                       '�X���V�����h�ƃT���v�����������M
        '�R�}���h���M
        bRet = WriteFile(hComm, wDATA, dLen, wLen, IntPtr.Zero)
        '�����₢���킹�p�^�C�}�J�n
        Timer1.Interval() = 500
        Timer1.Start()
        Timer1.Enabled = True
    End Sub

    Private Sub HScrollBar1_Scroll(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles HScrollBar1.Scroll
        '�������Ԏ��p�����[�^�擾
        HDIV = HScrollBar1.Value

    End Sub

    Private Sub HScrollBar2_Scroll(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles HScrollBar2.Scroll
        '�g���K�p�X���b�V�����h�p�����[�^�擾
        Threshold = HScrollBar2.Value
    End Sub

    Private Sub HScrollBar3_Scroll(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles HScrollBar3.Scroll
        '�T���v�������p�����[�^�擾
        Sample = HScrollBar3.Value
    End Sub

    Private Sub RadioButton1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton1.CheckedChanged
        If RadioButton1.Checked Then Channel = 0
    End Sub

    Private Sub RadioButton2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton2.CheckedChanged
        If RadioButton2.Checked Then Channel = 1
    End Sub

    Private Sub GroupBox1_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GroupBox1.Enter
        If RadioButton1.Checked Then Channel = 0
        If RadioButton2.Checked Then Channel = 1
    End Sub
End Class
