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
        Me.Button1.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Button1.Location = New System.Drawing.Point(576, 488)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(72, 32)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "更新"
        '
        'Timer1
        '
        '
        'Button2
        '
        Me.Button2.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Button2.Location = New System.Drawing.Point(672, 488)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(72, 32)
        Me.Button2.TabIndex = 1
        Me.Button2.Text = "消去"
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
        'グラフ表示制御
        'グラフ表示範囲設定と座標設定
        Dim W As Integer = ClientRectangle.Width - 20 '左端少し残す
        Dim H As Integer = ClientRectangle.Height - 20
        Dim G As Graphics = CreateGraphics()
        G.Clear(System.Drawing.Color.White)         '背景色　白
        G.TranslateTransform(20, H / 2)             '座標設定
        G.ScaleTransform(1, -1)                     'Y座標＋－逆転
        G.DrawLine(Pens.Blue, 0, 0, W, 0)           'X軸描画
        G.DrawLine(Pens.Blue, 0, -H \ 2, 0, H \ 2)  'Y軸描画
        'データの描画　直線で補完
        For X = 0 To Index - 1
            PY = mDATA(X) - Val(F1.TextBox5.Text)
            If X = 0 Then
                PX = 0
                oldX = PX
                oldY = 0            '初期値は0
            Else
                PX = PX + 1
                If PX > W Then      '右端になったら左端に戻る
                    PX = 0
                End If
                '直線で描画
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
        '描画指示
        Display()
        '２倍の間隔で再描画
        Timer1.Interval = Val(F1.TextBox4.Text) * 2000
        Timer1.Start()
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        '再描画実行
        Display()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        '消去処理
        Timer1.Stop()
        Me.Close()
    End Sub
End Class
