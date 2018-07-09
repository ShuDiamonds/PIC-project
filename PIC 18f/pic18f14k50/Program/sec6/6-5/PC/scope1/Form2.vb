Public Class Form2
    Inherits System.Windows.Forms.Form
    Dim X, PX, PY, oldX, oldY As Integer

#Region " Windows フォーム デザイナで生成されたコード "

    Public Sub New()
        MyBase.New()

        ' この呼び出しは Windows フォーム デザイナで必要です。
        InitializeComponent()

        ' InitializeComponent() 呼び出しの後に初期化を追加します。

    End Sub

    ' Form は、コンポーネント一覧に後処理を実行するために dispose をオーバーライドします。
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    ' Windows フォーム デザイナで必要です。
    Private components As System.ComponentModel.IContainer

    ' メモ : 以下のプロシージャは、Windows フォーム デザイナで必要です。
    'Windows フォーム デザイナを使って変更してください。  
    ' コード エディタを使って変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        '
        'Form2
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 12)
        Me.BackColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.ClientSize = New System.Drawing.Size(976, 366)
        Me.Name = "Form2"
        Me.Text = "オシロスコープ"

    End Sub

#End Region
    Private Sub Display()
        Dim I As Integer
        Dim Div As Single

        'グラフ表示制御
        'グラフ表示範囲設定と座標設定
        Dim W As Integer = ClientRectangle.Width - 20 '左端少し残す
        Dim H As Integer = ClientRectangle.Height - 20
        Dim G As Graphics = CreateGraphics()
        Dim myFont As New Font("Courier", 14, FontStyle.Regular)
        Dim myBrush As New SolidBrush(Color.Black)

        G.Clear(System.Drawing.Color.White)         '背景色　白
        G.TranslateTransform(20, H / 2)             '座標設定
        '計測単位表示
        Div = Sample * 1 / HDIV
        G.DrawString(Format(Div, "##.##") + "msec/Div", myFont, myBrush, 10, 165)
        G.DrawString("0V", myFont, myBrush, -19, -10)
        'グラフ表示用に設定
        G.ScaleTransform(1, -1)                     'Y座標＋－逆転
        '補助線と座標線描画
        For I = -1 To 2 Step 1
            G.DrawLine(Pens.Gray, 0, I * 100, W, I * 100)
        Next
        G.DrawLine(Pens.Blue, 0, 0, W, 0)           'X軸描画
        For I = 1 To 10 Step 1
            G.DrawLine(Pens.Gray, I * 100, -H \ 2, I * 100, H \ 2)
        Next
        G.DrawLine(Pens.Blue, 0, -H \ 2, 0, H \ 2)  'Y軸描画
        'データの描画　直線で補完
        For X = 0 To Index - 1
            PY = mDATA(X) - 127                     '中央値127とする
            If X = 0 Then
                PX = 0
                oldX = PX
                oldY = mDATA(0) - 127               '初期値
            Else
                PX = PX + HDIV
                '直線で描画
                G.DrawLine(Pens.Red, oldX, oldY, PX, PY)
                oldX = PX
                oldY = PY
            End If
        Next
    End Sub

    Private Sub Form2_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Activated
        '描画指示
        Application.DoEvents()
        Display()
    End Sub

    Private Sub Form2_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        F2 = Me
        '描画指示
        Application.DoEvents()
        Display()

    End Sub



End Class
