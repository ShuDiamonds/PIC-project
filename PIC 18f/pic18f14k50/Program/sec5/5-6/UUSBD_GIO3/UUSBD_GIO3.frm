VERSION 5.00
Begin VB.Form Form1 
   Caption         =   "�ėpUSB_GIO(����IF)"
   ClientHeight    =   4065
   ClientLeft      =   60
   ClientTop       =   390
   ClientWidth     =   4590
   LinkTopic       =   "Form1"
   ScaleHeight     =   4065
   ScaleWidth      =   4590
   StartUpPosition =   3  'Windows �̊���l
   Begin VB.Timer Timer2 
      Left            =   3840
      Top             =   2280
   End
   Begin VB.TextBox Text6 
      Alignment       =   2  '��������
      BeginProperty Font 
         Name            =   "�l�r �o�S�V�b�N"
         Size            =   14.25
         Charset         =   128
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   1680
      TabIndex        =   13
      Text            =   "Data"
      Top             =   2280
      Width           =   2055
   End
   Begin VB.CommandButton Command8 
      Caption         =   "CH0 In"
      Height          =   375
      Left            =   360
      TabIndex        =   12
      Top             =   2280
      Width           =   1095
   End
   Begin VB.Timer Timer1 
      Left            =   3840
      Top             =   1680
   End
   Begin VB.TextBox Text5 
      Alignment       =   2  '��������
      BeginProperty Font 
         Name            =   "�l�r �o�S�V�b�N"
         Size            =   14.25
         Charset         =   128
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   1680
      TabIndex        =   11
      Text            =   "Data"
      Top             =   1680
      Width           =   2055
   End
   Begin VB.CommandButton Command7 
      Caption         =   "POT"
      Height          =   375
      Left            =   360
      TabIndex        =   10
      Top             =   1680
      Width           =   1095
   End
   Begin VB.TextBox Text4 
      Alignment       =   2  '��������
      BeginProperty Font 
         Name            =   "�l�r �o�S�V�b�N"
         Size            =   14.25
         Charset         =   128
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   1680
      TabIndex        =   9
      Text            =   "Result"
      Top             =   120
      Width           =   975
   End
   Begin VB.CommandButton Command6 
      Caption         =   "USB�ڑ�"
      Height          =   375
      Left            =   360
      TabIndex        =   8
      Top             =   120
      Width           =   1095
   End
   Begin VB.CommandButton Command5 
      Caption         =   "LED2"
      Height          =   375
      Left            =   360
      TabIndex        =   7
      Top             =   1200
      Width           =   1095
   End
   Begin VB.TextBox Text3 
      Alignment       =   2  '��������
      BackColor       =   &H0000FF00&
      BeginProperty Font 
         Name            =   "�l�r �o�S�V�b�N"
         Size            =   14.25
         Charset         =   128
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   1680
      TabIndex        =   6
      Text            =   "LED2"
      Top             =   1200
      Width           =   1095
   End
   Begin VB.TextBox Text2 
      BeginProperty Font 
         Name            =   "�l�r �o�S�V�b�N"
         Size            =   12
         Charset         =   128
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   1680
      TabIndex        =   5
      Text            =   "ABCDEFGHIJKLMNOP"
      Top             =   2880
      Width           =   2535
   End
   Begin VB.CommandButton Command4 
      Caption         =   "LCD Clear"
      Height          =   375
      Left            =   360
      TabIndex        =   4
      Top             =   3480
      Width           =   1095
   End
   Begin VB.CommandButton Command3 
      Caption         =   "LCD"
      Height          =   375
      Left            =   360
      TabIndex        =   3
      Top             =   2880
      Width           =   1095
   End
   Begin VB.CommandButton Command2 
      Caption         =   "LED1"
      Height          =   375
      Left            =   360
      TabIndex        =   2
      Top             =   720
      Width           =   1095
   End
   Begin VB.CommandButton Command1 
      Caption         =   "�I��"
      Height          =   375
      Left            =   3120
      TabIndex        =   1
      Top             =   120
      Width           =   1215
   End
   Begin VB.TextBox Text1 
      Alignment       =   2  '��������
      BackColor       =   &H0000FF00&
      BeginProperty Font 
         Name            =   "�l�r �o�S�V�b�N"
         Size            =   14.25
         Charset         =   128
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   420
      Left            =   1680
      TabIndex        =   0
      Text            =   "LED1"
      Top             =   720
      Width           =   1095
   End
End
Attribute VB_Name = "Form1"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Private Sub Command6_Click()
'
'   �ėpI/O�@USB����@������
'
    hUSB = Uusbd_Open_mask(UU_MASK_VENDOR + UU_MASK_PRODUCT, 0, 0, Vendor, Product, 0)
'    hUSB = Uusbd_Open()
    If hUSB = -1 Then
        MsgBox "USB�ڑ����ł��܂���"
        End
    End If
    ' �C���^�[�t�F�[�X�O���̃p�C�v�̃I�[�v��
    hCMD = Uusbd_OpenPipe(hUSB, 0, 0)
    If hCMD = -1 Then
        MsgBox "�o�̓p�C�v�O���J���܂���ł���"
        End
    End If
    hSTA = Uusbd_OpenPipe(hUSB, 0, 1)
    If hSTA = -1 Then
        MsgBox "���̓p�C�v�P���J���܂���ł���"
        End
    End If
    ' �C���^�[�t�F�[�X�P���̃p�C�v�I�[�v��
    hOUT = Uusbd_OpenPipe(hUSB, 1, 0)
    If hOUT = -1 Then
        MsgBox "�o�̓p�C�v�Q���J���܂���ł���"
        End
    End If
    hINP = Uusbd_OpenPipe(hUSB, 1, 1)
    If hINP = -1 Then
        MsgBox "���̓p�C�v�R���J���܂���ł���"
        End
    End If
    ' ����I�[�v�����b�Z�[�W
    Text4.Text = "Ok!"
    ' �^�C�}�P�C�Q��~
    Timer1.Enabled = False
    Timer2.Enabled = False
End Sub

Private Sub Command1_Click()
'
'   �v���O�����I��
'
    CloseHandle (hINP)      '�o���NOUT�p�C�v�T�N���[�Y
    CloseHandle (hSTA)      '�o���NIN�p�C�v�S�N���[�Y
    CloseHandle (hOUT)      '�o���NOUT�p�C�v�R�N���[�Y
    CloseHandle (hCMD)      '�o���NOUT�p�C�v�P�N���[�Y
    Uusbd_Close (hUSB)      'USB�f�o�C�X�N���[�Y
    End
End Sub

Private Sub Command2_Click()
'
' LED1�̃I���I�t����i�C���^�[�t�F�[�X�P���j
'
    Dim Command(32) As Byte
    Dim RcvData(32) As Byte
    Dim Result As Long
    Dim State As Long
    ' �R�}���h���M
    Command(0) = &H31
    State = WriteFile(hOUT, Command(0), 1, Result, 0)
    If State = 0 Then   '�G���[������
        MsgBox "�o�̓p�C�v�Q�̏������݂ŃG���[������"
        End
    End If
    ' �@�܂�Ԃ��|�[�g��ԓ���
    State = ReadFile(hINP, RcvData(0), 32, Result, 0)
    If State = 0 Then
        MsgBox "���̓p�C�v�R�̓ǂݏo���ŃG���[������"
        End
    End If
    ' LED��ԕ\��
    If RcvData(0) = &H30 Then
        Text1.BackColor = &HFF&          '�ԕ\��
    Else
        Text1.BackColor = &HFF00&        '�Ε\��
    End If
End Sub

Private Sub Command3_Click()
'
' �t���\����̕\������i�C���^�[�t�F�[�X�O���j
'
    Dim Command(64) As Byte
    Dim Result As Long
    Dim State As Long
    ' �R�}���h���M
    Command(0) = &H33
    Command(1) = &HC0
    Command(2) = 1
    '�Œ��������P�U�Ő���
    Text2.Text = Left((Text2.Text & "                "), 16)
    'UNICODE����ASCII�ɕϊ������M�o�b�t�@�Ɋi�[
    For i = 1 To 16
        Command(i + 2) = Asc(Mid(Text2.Text, i, 1))
    Next
    '�f�[�^�I���}�[�N�i�[
    Command(19) = 0
    '�f�[�^���M
    State = WriteFile(hCMD, Command(0), 20, Result, 0)
    If State = 0 Then   '�G���[������
        MsgBox "�o�̓p�C�v�O�̏������݂ŃG���[������"
        End
    End If

End Sub

Private Sub Command4_Click()
'
' �t���\����̕\���N���A����
'
    Dim Command(5) As Byte
    Dim Result As Long
    Dim State As Long
    ' �R�}���h���M
    Command(0) = &H34
    State = WriteFile(hCMD, Command(0), 1, Result, 0)
    If State = 0 Then   '�G���[������
        MsgBox "�o�̓p�C�v�O�̏������݂ŃG���[������"
        End
    End If
End Sub

Private Sub Command5_Click()
'
' LED2�̃I���I�t����i�C���^�[�t�F�[�X�O���j
'
    Dim Command(32) As Byte
    Dim RcvData(32) As Byte
    Dim Result As Long
    Dim State As Long
    ' �R�}���h���M
    Command(0) = &H32
    State = WriteFile(hCMD, Command(0), 1, Result, 0)
    If State = 0 Then   '�G���[������
        MsgBox "�o�̓p�C�v�O�̏������݂ŃG���[������"
        End
    End If
    ' �@�܂�Ԃ��|�[�g��ԓ���
    State = ReadFile(hSTA, RcvData(0), 32, Result, 0)
    If State = 0 Then
        MsgBox "���̓p�C�v�P�̓ǂݏo���ŃG���[������"
        End
    End If
    '�@LED��ԕ\��
    If RcvData(0) = &H30 Then
        Text3.BackColor = &HFF&          '�ԕ\��
    Else
        Text3.BackColor = &HFF00&        '�Ε\��
    End If
End Sub

Private Sub Command7_Click()
'
' �ϒ�R�̈ʒu�ǂݍ��݁i�C���^�[�t�F�[�X�O���j
'
    Dim Command(32) As Byte
    Dim RcvData(32) As Byte
    Dim Result As Long
    Dim State As Long
    ' �R�}���h���M
    Command(0) = &H35
    State = WriteFile(hCMD, Command(0), 1, Result, 0)
    If State = 0 Then   '�G���[������
        MsgBox "�o�̓p�C�v�O�̏������݂ŃG���[������"
        End
    End If
    ' �@�܂�Ԃ�A/D�ϊ����ʓ���
    State = ReadFile(hSTA, RcvData(0), 32, Result, 0)
    If State = 0 Then
        MsgBox "���̓p�C�v�P�̓ǂݏo���ŃG���[������"
        End
    End If
    '��M�f�[�^�\��
    Text5.Text = ""
    For i = 0 To 12
        Text5.Text = Text5.Text & Chr(RcvData(i))
    Next
    '�J��Ԃ��\���J�n
    Timer1.Interval = 100
    Timer1.Enabled = True
End Sub

Private Sub Command8_Click()
'
' �`���l���O�̓ǂݍ��݁i�C���^�[�t�F�[�X�P���j
'
    Dim Command(32) As Byte
    Dim RcvData(32) As Byte
    Dim Result As Long
    Dim State As Long
    ' �R�}���h���M
    Command(0) = &H35
    State = WriteFile(hOUT, Command(0), 1, Result, 0)
    If State = 0 Then   '�G���[������
        MsgBox "�o�̓p�C�v�O�̏������݂ŃG���[������"
        End
    End If
    ' �@�܂�Ԃ�A/D�ϊ����ʓ���
    State = ReadFile(hINP, RcvData(0), 32, Result, 0)
    If State = 0 Then
        MsgBox "���̓p�C�v�P�̓ǂݏo���ŃG���[������"
        End
    End If
    '��M�f�[�^�\��
    Text6.Text = ""
    For i = 0 To 12
        Text6.Text = Text6.Text & Chr(RcvData(i))
    Next
    '�J��Ԃ��\���J�n
    Timer2.Interval = 90
    Timer2.Enabled = True

End Sub

Private Sub Form_Unload(Cancel As Integer)
'
'   �v���O�����I��
'
    CloseHandle (hINP)      '�o���NOUT�p�C�v�T�N���[�Y
    CloseHandle (hSTA)      '�o���NIN�p�C�v�S�N���[�Y
    CloseHandle (hOUT)      '�o���NOUT�p�C�v�R�N���[�Y
    CloseHandle (hCMD)      '�o���NOUT�p�C�v�P�N���[�Y
    Uusbd_Close (hUSB)      'USB�f�o�C�X�N���[�Y
    End
End Sub

Private Sub Timer1_Timer()
'
' �ϒ�R�̈ʒu�ǂݍ���
'
    Dim Command(32) As Byte
    Dim RcvData(32) As Byte
    Dim Result As Long
    Dim State As Long
    ' �R�}���h���M
    Command(0) = &H35
    State = WriteFile(hCMD, Command(0), 1, Result, 0)
    If State = 0 Then   '�G���[������
        MsgBox "�o�̓p�C�v�O�̏������݂ŃG���[������"
        End
    End If
    ' �@�܂�Ԃ�A/D�ϊ����ʓ���
    State = ReadFile(hSTA, RcvData(0), 32, Result, 0)
    If State = 0 Then
        MsgBox "���̓p�C�v�P�̓ǂݏo���ŃG���[������"
        End
    End If
    '��M�f�[�^�\��
    Text5.Text = ""
    For i = 0 To 12
        Text5.Text = Text5.Text & Chr(RcvData(i))
    Next
End Sub

Private Sub Timer2_Timer()
'
' �`���l���O�̓ǂݍ���
'
    Dim Command(32) As Byte
    Dim RcvData(32) As Byte
    Dim Result As Long
    Dim State As Long
    ' �R�}���h���M
    Command(0) = &H35
    State = WriteFile(hOUT, Command(0), 1, Result, 0)
    If State = 0 Then   '�G���[������
        MsgBox "�o�̓p�C�v�O�̏������݂ŃG���[������"
        End
    End If
    ' �@�܂�Ԃ�A/D�ϊ����ʓ���
    State = ReadFile(hINP, RcvData(0), 32, Result, 0)
    If State = 0 Then
        MsgBox "���̓p�C�v�P�̓ǂݏo���ŃG���[������"
        End
    End If
    '��M�f�[�^�\��
    Text6.Text = ""
    For i = 0 To 12
        Text6.Text = Text6.Text & Chr(RcvData(i))
    Next
End Sub
