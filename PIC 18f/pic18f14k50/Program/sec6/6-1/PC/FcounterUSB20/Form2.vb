Public Class Form2
    Inherits System.Windows.Forms.Form
    Dim X, PX, PY, oldX, oldY As Integer


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
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents Button2 As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.Button1 = New System.Windows.Forms.Button
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.Button2 = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.Font = New System.Drawing.Font("�l�r �S�V�b�N", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Button1.Location = New System.Drawing.Point(576, 488)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(72, 32)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "�X�V"
        '
        'Timer1
        '
        '
        'Button2
        '
        Me.Button2.Font = New System.Drawing.Font("�l�r �S�V�b�N", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Button2.Location = New System.Drawing.Point(672, 488)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(72, 32)
        Me.Button2.TabIndex = 1
        Me.Button2.Text = "����"
        '
        'Form2
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 12)
        Me.ClientSize = New System.Drawing.Size(776, 534)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Name = "Form2"
        Me.Text = "Form2"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub Display()
        '�O���t�\������
        '�O���t�\���͈͐ݒ�ƍ��W�ݒ�
        Dim W As Integer = ClientRectangle.Width - 20 '���[�����c��
        Dim H As Integer = ClientRectangle.Height - 20
        Dim G As Graphics = CreateGraphics()
        G.Clear(System.Drawing.Color.White)         '�w�i�F�@��
        G.TranslateTransform(20, H / 2)             '���W�ݒ�
        G.ScaleTransform(1, -1)                     'Y���W�{�|�t�]
        G.DrawLine(Pens.Blue, 0, 0, W, 0)           'X���`��
        G.DrawLine(Pens.Blue, 0, -H \ 2, 0, H \ 2)  'Y���`��
        '�f�[�^�̕`��@�����ŕ⊮
        For X = 0 To Index - 1
            PY = mDATA(X) - Val(F1.TextBox5.Text)
            If X = 0 Then
                PX = 0
                oldX = PX
                oldY = 0            '�����l��0
            Else
                PX = PX + 1
                If PX > W Then      '�E�[�ɂȂ����獶�[�ɖ߂�
                    PX = 0
                End If
                '�����ŕ`��
                G.DrawLine(Pens.Red, oldX, oldY, PX, PY)
                oldX = PX
                oldY = PY
            End If
        Next
    End Sub
    Private Sub Form2_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        F2 = Me
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        '�`��w��
        Display()
        '�Q�{�̊Ԋu�ōĕ`��
        Timer1.Interval = Val(F1.TextBox4.Text) * 2000
        Timer1.Start()
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        '�ĕ`����s
        Display()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        '��������
        Timer1.Stop()
        Me.Close()
    End Sub
End Class
