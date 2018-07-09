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
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        '
        'Form2
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 12)
        Me.BackColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.ClientSize = New System.Drawing.Size(976, 366)
        Me.Name = "Form2"
        Me.Text = "�I�V���X�R�[�v"

    End Sub

#End Region
    Private Sub Display()
        Dim I As Integer
        Dim Div As Single

        '�O���t�\������
        '�O���t�\���͈͐ݒ�ƍ��W�ݒ�
        Dim W As Integer = ClientRectangle.Width - 20 '���[�����c��
        Dim H As Integer = ClientRectangle.Height - 20
        Dim G As Graphics = CreateGraphics()
        Dim myFont As New Font("Courier", 14, FontStyle.Regular)
        Dim myBrush As New SolidBrush(Color.Black)

        G.Clear(System.Drawing.Color.White)         '�w�i�F�@��
        G.TranslateTransform(20, H / 2)             '���W�ݒ�
        '�v���P�ʕ\��
        Div = Sample * 1 / HDIV
        G.DrawString(Format(Div, "##.##") + "msec/Div", myFont, myBrush, 10, 165)
        G.DrawString("0V", myFont, myBrush, -19, -10)
        '�O���t�\���p�ɐݒ�
        G.ScaleTransform(1, -1)                     'Y���W�{�|�t�]
        '�⏕���ƍ��W���`��
        For I = -1 To 2 Step 1
            G.DrawLine(Pens.Gray, 0, I * 100, W, I * 100)
        Next
        G.DrawLine(Pens.Blue, 0, 0, W, 0)           'X���`��
        For I = 1 To 10 Step 1
            G.DrawLine(Pens.Gray, I * 100, -H \ 2, I * 100, H \ 2)
        Next
        G.DrawLine(Pens.Blue, 0, -H \ 2, 0, H \ 2)  'Y���`��
        '�f�[�^�̕`��@�����ŕ⊮
        For X = 0 To Index - 1
            PY = mDATA(X) - 127                     '�����l127�Ƃ���
            If X = 0 Then
                PX = 0
                oldX = PX
                oldY = mDATA(0) - 127               '�����l
            Else
                PX = PX + HDIV
                '�����ŕ`��
                G.DrawLine(Pens.Red, oldX, oldY, PX, PY)
                oldX = PX
                oldY = PY
            End If
        Next
    End Sub

    Private Sub Form2_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Activated
        '�`��w��
        Application.DoEvents()
        Display()
    End Sub

    Private Sub Form2_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        F2 = Me
        '�`��w��
        Application.DoEvents()
        Display()

    End Sub



End Class
