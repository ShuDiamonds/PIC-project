'Socketクラスの呼び出し
Imports System
Imports System.Net
Imports System.Net.Sockets
Imports System.Text

Public Class Form1
    Inherits System.Windows.Forms.Form

    'Socketクラス インスタンス生成
    Dim UdpClientA As New UdpClient()
    '受信ポート定義（任意アドレス）
    Dim Remote As New IPEndPoint(IPAddress.Any, 0)

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        '特定リモートの接続（名前とポート番号で指定）
        Try
            UdpClientA.Connect(TextBox9.Text, 10002)
            TextBox1.Text = "Connect!"
        Catch ex As Exception
            TextBox1.Text = "Not Connected?"
        End Try
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Timer1.Enabled = False      'タイマ１停止
        UdpClientA.Close()          'UDPのクローズ
        Close()
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        'コマンドデータ送信
        Me.Enabled = False
        Try
            '現在状態によりオンオフを反転制御
            If Button3.BackColor = Color.Red Then
                UdpClientA.Send(LED1Off, LED1Off.Length)
            Else
                UdpClientA.Send(LED1On, LED1On.Length)
            End If
            '折り返しデータ受信（MLXXE)
            receiveBytes = UdpClientA.Receive(Remote)
            'オンオフ状態により色とテキストを変更
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
        'コマンドデータ送信
        Me.Enabled = False
        Try
            '現在状態によりオンオフを反転制御
            If Button4.BackColor = Color.Red Then
                UdpClientA.Send(LED2Off, LED2Off.Length)
            Else
                UdpClientA.Send(LED2On, LED2On.Length)
            End If
            '折り返しデータ受信(MLXXE)
            receiveBytes = UdpClientA.Receive(Remote)
            'オンオフ状態により色を変更
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
            'スイッチ状態入力コマンド送信
            UdpClientA.Send(SWInput, SWInput.Length)
            '折り返し受信（MBXXXX)
            receiveBytes = UdpClientA.Receive(Remote)
            'Xが0か1でオンオフ表示制御
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
            Timer1.Interval() = 500     '500msecタイマ起動
            Timer1.Enabled() = True     'タイマ１起動
            Button6.Text = "停止"
        Else
            Timer1.Enabled = False
            Button6.Text = "計測"

        End If
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Me.Enabled = False
        Try
            'チャネル1計測()
            UdpClientA.Send(AN2Mesure, AN2Mesure.Length)
            '折り返しデータ受信（MAXXXX)
            receiveBytes = UdpClientA.Receive(Remote)
            'データ変換と表示
            receiveBytes(0) = &H20              'Mをスペースに変更
            receiveBytes(1) = &H20              'Aをスペースに変更
            returnData = Encoding.ASCII.GetString(receiveBytes)
            TextBox6.Text = returnData.ToString()
            Application.DoEvents()              'テキストボックス表示を可能化
            'チャネル2計測
            UdpClientA.Send(AN3Mesure, AN3Mesure.Length)
            '折り返しデータ受信（MAXXXX)
            receiveBytes = UdpClientA.Receive(Remote)
            '計測データ変換と表示
            receiveBytes(0) = &H20              'Mをスペースに変更
            receiveBytes(1) = &H20              'Aをスペースに変更
            returnData = Encoding.ASCII.GetString(receiveBytes)
            TextBox7.Text = returnData.ToString()
            Application.DoEvents()              'テキストボックス表示を可能化
        Catch ex As Exception
            TextBox1.Text = ex.ToString()
        End Try
        Me.Enabled = True
    End Sub

    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
        Me.Enabled = False
        Try
            '液晶表示器への表示制御
            Dim SendMessage As Byte()
            SendMessage = Encoding.ASCII.GetBytes("SD" + TextBox8.Text)
            UdpClientA.Send(SendMessage, SendMessage.Length)
            '折り返しデータ受信（M?)
            receiveBytes = UdpClientA.Receive(Remote)
            'データ変換と表示
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
            '液晶表示器の消去
            UdpClientA.Send(LCDErase, LCDErase.Length)
            '折り返しデータ受信（M?)
            receiveBytes = UdpClientA.Receive(Remote)
            'データ変換と表示
            returnData = Encoding.ASCII.GetString(receiveBytes)
            TextBox1.Text = returnData.ToString()
        Catch ex As Exception
            TextBox1.Text = ex.ToString()
        End Try
        Me.Enabled = True
    End Sub

    '共通変数の定義
    Dim receiveBytes As Byte()
    Dim returnData As String

    '送信コマンドデータ定義
    Dim LED1On As Byte() = Encoding.ASCII.GetBytes("SC11")
    Dim LED1Off As Byte() = Encoding.ASCII.GetBytes("SC10")
    Dim LED2On As Byte() = Encoding.ASCII.GetBytes("SC21")
    Dim LED2Off As Byte() = Encoding.ASCII.GetBytes("SC20")
    Dim AN2Mesure As Byte() = Encoding.ASCII.GetBytes("SA1")
    Dim AN3Mesure As Byte() = Encoding.ASCII.GetBytes("SA2")
    Dim SWInput As Byte() = Encoding.ASCII.GetBytes("SB")
    Dim LCDErase As Byte() = Encoding.ASCII.GetBytes("SE")

End Class
