const crearPedido = () => {
    var myHeaders = new Headers();
    myHeaders.append("Content-Type", "application/json");

    var raw = JSON.stringify({
        "ClienteId": 1,
        "Descripcion": "sample string 2",
        "CondicionVenta": "sample string 3",
        "Estado": "sample string 4",
        "Pedidos": [
            {
                "ProductoId": 1,
                "CantidadProducto": 2,
                "CantidadCuotas": 3
            },
            {
                "ProductoId": 1,
                "CantidadProducto": 2,
                "CantidadCuotas": 3
            }
        ]
    });

    var requestOptions = {
        method: 'POST',
        headers: myHeaders,
        body: raw,
        redirect: 'follow'
    };

    fetch("https://cuentasctacte-web-api20220425205158.azurewebsites.net/Pedidos", requestOptions)
        .then(response => response.text())
        .then(result => console.log(result))
        .catch(error => console.log('error', error));
}
export default crearPedido