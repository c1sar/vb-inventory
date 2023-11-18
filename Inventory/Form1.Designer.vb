Imports MySql.Data.MySqlClient

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        InitializeCustomControls()
    End Sub

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(443, 418)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(203, 58)
        Me.Button1.TabIndex = 1
        Me.Button1.Text = "Generate purchase order"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(95, 38)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(122, 20)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Product Name"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(275, 38)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(116, 20)
        Me.Label4.TabIndex = 5
        Me.Label4.Text = "Product Price"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(439, 38)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(143, 20)
        Me.Label5.TabIndex = 6
        Me.Label5.Text = "Product Quantity"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(715, 538)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Button1)
        Me.Name = "Form1"
        Me.Text = "Ordering System"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Private Sub InitializeCustomControls()
        Dim yPosition As Integer = 60

        LoadDataFromDatabase()

        ReDim productControlGroups(productArray.Length - 1)
        For i As Integer = 0 To productArray.Length - 1

            productControlGroups(i) = New ProductControlGroup With {
            .ProductName = New Label(),
            .ProductPrice = New Label(),
            .ProductQuantity = New TextBox()
            }

            productControlGroups(i).ProductName.Text = productArray(i).Name
            productControlGroups(i).ProductName.Location = New Point(60, yPosition)

            productControlGroups(i).ProductPrice.Text = productArray(i).Price
            productControlGroups(i).ProductPrice.Location = New Point(180, yPosition)

            productControlGroups(i).ProductQuantity.Text = ""
            productControlGroups(i).ProductQuantity.Location = New Point(300, yPosition)

            ' Optionally set other properties like position, size, etc.

            Me.Controls.Add(productControlGroups(i).ProductName)
            Me.Controls.Add(productControlGroups(i).ProductPrice)
            Me.Controls.Add(productControlGroups(i).ProductQuantity)

            AddHandler productControlGroups(i).ProductQuantity.KeyPress, AddressOf TextBox_KeyPress

            yPosition = yPosition + 30
        Next
    End Sub

    Private Sub TextBox_KeyPress(sender As Object, e As KeyPressEventArgs)
        If Not Char.IsDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    Private Sub LoadDataFromDatabase()
        Dim connectionString As String = "Server=127.0.0.1; Database=nicxz_db; Uid=root;"
        Dim query As String = "SELECT * FROM product"

        Using connection As New MySqlConnection(connectionString)
            Try
                connection.Open()

                Dim command As New MySqlCommand(query, connection)
                Dim reader As MySqlDataReader = command.ExecuteReader()

                Dim resultList As New List(Of Product)()

                While reader.Read()

                    Dim product As New Product()
                    product.Id = Convert.ToInt32(reader("id"))
                    product.Name = reader("name").ToString()
                    product.Price = Convert.ToInt32(reader("price"))

                    resultList.Add(product)

                    For i As Integer = 0 To reader.FieldCount - 1
                        Console.Write(reader(i).ToString() & " ")
                    Next
                    Console.WriteLine()
                End While

                reader.Close()
                productArray = resultList.ToArray()

            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message)
            Finally
                connection.Close()
            End Try
        End Using
    End Sub
    Friend WithEvents Button1 As Button
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Dim productControlGroups As ProductControlGroup()
    Dim productArray As Product()
End Class

Public Class ProductControlGroup
    Public ProductName As Label
    Public ProductPrice As Label
    Public ProductQuantity As TextBox
End Class

Public Class Product
    Public Property Id As Integer
    Public Property Name As String
    Public Property Price As Integer
End Class