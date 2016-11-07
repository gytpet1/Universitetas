<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="text-align: left">
    
        LDD_10. Pajamos.<br />
&nbsp;Pirmoje failo eilutėje nurodyta įvedimo data, o tolesnėse eilutėse nurodyta prenumeratoriaus pavardė,<br />
&nbsp;adresas, laikotarpio pradžia (nurodyta sveiku skaičiumi 1..12), laikotarpio ilgis, leidinio kodas,<br />
&nbsp;leidinių kiekis. Kitame faile duota tokia informacija apie leidinius: kodas, pavadinimas, leidėjo<br />
&nbsp;pavadinimas, vieno mėnesio kaina. Suskaičiuoti kiekvienam leidėjui nurodyto<br />
&nbsp;mėnesio (įvedama klaviatūra) pajamas. Atspausdinkite leidėjų pajamas, surikiuotas pagal
        <br />
        dydį ir leidėjų pavadinimu.<br />
        <br />
        Pasirinkite mėnesį:
        <asp:DropDownList ID="DropDownList1" runat="server" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
            <asp:ListItem Value="1" Selected="True">Sausis</asp:ListItem>
            <asp:ListItem Value="2">Vasaris</asp:ListItem>
            <asp:ListItem Value="3">Kovas</asp:ListItem>
            <asp:ListItem Value="4">Balandis</asp:ListItem>
            <asp:ListItem Value="5">Gegužė</asp:ListItem>
            <asp:ListItem Value="6">Birželis</asp:ListItem>
            <asp:ListItem Value="7">Liepa</asp:ListItem>
            <asp:ListItem Value="8">Rugpjūtis</asp:ListItem>
            <asp:ListItem Value="9">Rugsėjis</asp:ListItem>
            <asp:ListItem Value="10">Spalis</asp:ListItem>
            <asp:ListItem Value="11">Lapkritis</asp:ListItem>
            <asp:ListItem Value="12">Gruodis</asp:ListItem>
        </asp:DropDownList>
        <br />
        <br />
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Skaičiuoti pajamas" />
        <br />
        <br />
        Rezultatai bus išsaugomi &quot;Rezultatai.txt&quot; faile.</div>
    </form>
</body>
</html>
