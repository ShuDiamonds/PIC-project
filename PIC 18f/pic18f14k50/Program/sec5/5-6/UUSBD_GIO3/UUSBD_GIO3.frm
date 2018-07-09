VERSION 5.00
Begin VB.Form Form1 
   Caption         =   "汎用USB_GIO(複数IF)"
   ClientHeight    =   4065
   ClientLeft      =   60
   ClientTop       =   390
   ClientWidth     =   4590
   LinkTopic       =   "Form1"
   ScaleHeight     =   4065
   ScaleWidth      =   4590
   StartUpPosition =   3  'Windows の既定値
   Begin VB.Timer Timer2 
      Left            =   3840
      Top             =   2280
   End
   Begin VB.TextBox Text6 
      Alignment       =   2  '中央揃え
      BeginProperty Font 
         Name            =   "ＭＳ Ｐゴシック"
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
      Alignment       =   2  '中央揃え
      BeginProperty Font 
         Name            =   "ＭＳ Ｐゴシック"
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
      Alignment       =   2  '中央揃え
      BeginProperty Font 
         Name            =   "ＭＳ Ｐゴシック"
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
      Caption         =   "USB接続"
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
      Alignment       =   2  '中央揃え
      BackColor       =   &H0000FF00&
      BeginProperty Font 
         Name            =   "ＭＳ Ｐゴシック"
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
         Name            =   "ＭＳ Ｐゴシック"
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
      Caption         =   "終了"
      Height          =   375
      Left            =   3120
      TabIndex        =   1
      Top             =   120
      Width           =   1215
   End
   Begin VB.TextBox Text1 
      Alignment       =   2  '中央揃え
      BackColor       =   &H0000FF00&
      BeginProperty Font 
         Name            =   "ＭＳ Ｐゴシック"
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
'   汎用I/O　USB制御　初期化
'
    hUSB = Uusbd_Open_mask(UU_MASK_VENDOR + UU_MASK_PRODUCT, 0, 0, Vendor, Product, 0)
'    hUSB = Uusbd_Open()
    If hUSB = -1 Then
        MsgBox "USB接続ができません"
        End
    End If
    ' インターフェース０側のパイプのオープン
    hCMD = Uusbd_OpenPipe(hUSB, 0, 0)
    If hCMD = -1 Then
        MsgBox "出力パイプ０を開けませんでした"
        End
    End If
    hSTA = Uusbd_OpenPipe(hUSB, 0, 1)
    If hSTA = -1 Then
        MsgBox "入力パイプ１を開けませんでした"
        End
    End If
    ' インターフェース１側のパイプオープン
    hOUT = Uusbd_OpenPipe(hUSB, 1, 0)
    If hOUT = -1 Then
        MsgBox "出力パイプ２を開けませんでした"
        End
    End If
    hINP = Uusbd_OpenPipe(hUSB, 1, 1)
    If hINP = -1 Then
        MsgBox "入力パイプ３を開けませんでした"
        End
    End If
    ' 正常オープンメッセージ
    Text4.Text = "Ok!"
    ' タイマ１，２停止
    Timer1.Enabled = False
    Timer2.Enabled = False
End Sub

Private Sub Command1_Click()
'
'   プログラム終了
'
    CloseHandle (hINP)      'バルクOUTパイプ５クローズ
    CloseHandle (hSTA)      'バルクINパイプ４クローズ
    CloseHandle (hOUT)      'バルクOUTパイプ３クローズ
    CloseHandle (hCMD)      'バルクOUTパイプ１クローズ
    Uusbd_Close (hUSB)      'USBデバイスクローズ
    End
End Sub

Private Sub Command2_Click()
'
' LED1のオンオフ制御（インターフェース１側）
'
    Dim Command(32) As Byte
    Dim RcvData(32) As Byte
    Dim Result As Long
    Dim State As Long
    ' コマンド送信
    Command(0) = &H31
    State = WriteFile(hOUT, Command(0), 1, Result, 0)
    If State = 0 Then   'エラー発生時
        MsgBox "出力パイプ２の書き込みでエラーが発生"
        End
    End If
    ' 　折り返しポート状態入力
    State = ReadFile(hINP, RcvData(0), 32, Result, 0)
    If State = 0 Then
        MsgBox "入力パイプ３の読み出しでエラーが発生"
        End
    End If
    ' LED状態表示
    If RcvData(0) = &H30 Then
        Text1.BackColor = &HFF&          '赤表示
    Else
        Text1.BackColor = &HFF00&        '緑表示
    End If
End Sub

Private Sub Command3_Click()
'
' 液晶表示器の表示制御（インターフェース０側）
'
    Dim Command(64) As Byte
    Dim Result As Long
    Dim State As Long
    ' コマンド送信
    Command(0) = &H33
    Command(1) = &HC0
    Command(2) = 1
    '最長文字数１６で制限
    Text2.Text = Left((Text2.Text & "                "), 16)
    'UNICODEからASCIIに変換し送信バッファに格納
    For i = 1 To 16
        Command(i + 2) = Asc(Mid(Text2.Text, i, 1))
    Next
    'データ終了マーク格納
    Command(19) = 0
    'データ送信
    State = WriteFile(hCMD, Command(0), 20, Result, 0)
    If State = 0 Then   'エラー発生時
        MsgBox "出力パイプ０の書き込みでエラーが発生"
        End
    End If

End Sub

Private Sub Command4_Click()
'
' 液晶表示器の表示クリア制御
'
    Dim Command(5) As Byte
    Dim Result As Long
    Dim State As Long
    ' コマンド送信
    Command(0) = &H34
    State = WriteFile(hCMD, Command(0), 1, Result, 0)
    If State = 0 Then   'エラー発生時
        MsgBox "出力パイプ０の書き込みでエラーが発生"
        End
    End If
End Sub

Private Sub Command5_Click()
'
' LED2のオンオフ制御（インターフェース０側）
'
    Dim Command(32) As Byte
    Dim RcvData(32) As Byte
    Dim Result As Long
    Dim State As Long
    ' コマンド送信
    Command(0) = &H32
    State = WriteFile(hCMD, Command(0), 1, Result, 0)
    If State = 0 Then   'エラー発生時
        MsgBox "出力パイプ０の書き込みでエラーが発生"
        End
    End If
    ' 　折り返しポート状態入力
    State = ReadFile(hSTA, RcvData(0), 32, Result, 0)
    If State = 0 Then
        MsgBox "入力パイプ１の読み出しでエラーが発生"
        End
    End If
    '　LED状態表示
    If RcvData(0) = &H30 Then
        Text3.BackColor = &HFF&          '赤表示
    Else
        Text3.BackColor = &HFF00&        '緑表示
    End If
End Sub

Private Sub Command7_Click()
'
' 可変抵抗の位置読み込み（インターフェース０側）
'
    Dim Command(32) As Byte
    Dim RcvData(32) As Byte
    Dim Result As Long
    Dim State As Long
    ' コマンド送信
    Command(0) = &H35
    State = WriteFile(hCMD, Command(0), 1, Result, 0)
    If State = 0 Then   'エラー発生時
        MsgBox "出力パイプ０の書き込みでエラーが発生"
        End
    End If
    ' 　折り返しA/D変換結果入力
    State = ReadFile(hSTA, RcvData(0), 32, Result, 0)
    If State = 0 Then
        MsgBox "入力パイプ１の読み出しでエラーが発生"
        End
    End If
    '受信データ表示
    Text5.Text = ""
    For i = 0 To 12
        Text5.Text = Text5.Text & Chr(RcvData(i))
    Next
    '繰り返し表示開始
    Timer1.Interval = 100
    Timer1.Enabled = True
End Sub

Private Sub Command8_Click()
'
' チャネル０の読み込み（インターフェース１側）
'
    Dim Command(32) As Byte
    Dim RcvData(32) As Byte
    Dim Result As Long
    Dim State As Long
    ' コマンド送信
    Command(0) = &H35
    State = WriteFile(hOUT, Command(0), 1, Result, 0)
    If State = 0 Then   'エラー発生時
        MsgBox "出力パイプ０の書き込みでエラーが発生"
        End
    End If
    ' 　折り返しA/D変換結果入力
    State = ReadFile(hINP, RcvData(0), 32, Result, 0)
    If State = 0 Then
        MsgBox "入力パイプ１の読み出しでエラーが発生"
        End
    End If
    '受信データ表示
    Text6.Text = ""
    For i = 0 To 12
        Text6.Text = Text6.Text & Chr(RcvData(i))
    Next
    '繰り返し表示開始
    Timer2.Interval = 90
    Timer2.Enabled = True

End Sub

Private Sub Form_Unload(Cancel As Integer)
'
'   プログラム終了
'
    CloseHandle (hINP)      'バルクOUTパイプ５クローズ
    CloseHandle (hSTA)      'バルクINパイプ４クローズ
    CloseHandle (hOUT)      'バルクOUTパイプ３クローズ
    CloseHandle (hCMD)      'バルクOUTパイプ１クローズ
    Uusbd_Close (hUSB)      'USBデバイスクローズ
    End
End Sub

Private Sub Timer1_Timer()
'
' 可変抵抗の位置読み込み
'
    Dim Command(32) As Byte
    Dim RcvData(32) As Byte
    Dim Result As Long
    Dim State As Long
    ' コマンド送信
    Command(0) = &H35
    State = WriteFile(hCMD, Command(0), 1, Result, 0)
    If State = 0 Then   'エラー発生時
        MsgBox "出力パイプ０の書き込みでエラーが発生"
        End
    End If
    ' 　折り返しA/D変換結果入力
    State = ReadFile(hSTA, RcvData(0), 32, Result, 0)
    If State = 0 Then
        MsgBox "入力パイプ１の読み出しでエラーが発生"
        End
    End If
    '受信データ表示
    Text5.Text = ""
    For i = 0 To 12
        Text5.Text = Text5.Text & Chr(RcvData(i))
    Next
End Sub

Private Sub Timer2_Timer()
'
' チャネル０の読み込み
'
    Dim Command(32) As Byte
    Dim RcvData(32) As Byte
    Dim Result As Long
    Dim State As Long
    ' コマンド送信
    Command(0) = &H35
    State = WriteFile(hOUT, Command(0), 1, Result, 0)
    If State = 0 Then   'エラー発生時
        MsgBox "出力パイプ０の書き込みでエラーが発生"
        End
    End If
    ' 　折り返しA/D変換結果入力
    State = ReadFile(hINP, RcvData(0), 32, Result, 0)
    If State = 0 Then
        MsgBox "入力パイプ１の読み出しでエラーが発生"
        End
    End If
    '受信データ表示
    Text6.Text = ""
    For i = 0 To 12
        Text6.Text = Text6.Text & Chr(RcvData(i))
    Next
End Sub
