const agregarPedido = (token, raw) => {
    const myHeaders = new Headers();
    myHeaders.append("Authorization", `Bearer ${token}`);
    myHeaders.append("Content-Type", "application/json");

    const requestOptions = {
        method: 'POST',
        headers: myHeaders,
        body: raw,
        redirect: 'follow'
    };

    fetch("https://cuentasctacte-web-api20220425205158.azurewebsites.net/api/Pedidos", requestOptions)
        .then(response => response.text())
        .then(result => console.log(JSON.parse(result)))
        .catch(error => alert(error.Message));
}
export default agregarPedido