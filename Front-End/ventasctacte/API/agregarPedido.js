const agregarPedido = (token) => {
    var myHeaders = new Headers();
    myHeaders.append("Authorization", `Bearer ${token}`);
    myHeaders.append("Content-Type", "application/json");

    var raw = JSON.stringify({
        "ClienteId": 1,
        "Descripcion": "sample string 2",
        "CondicionVenta": "sample string 3",
        "Estado": "PENDIENTE",
        "Pedidos": [
            {
                "ProductoId": 1,
                "CantidadProducto": 1,
                "CantidadCuotas": 3
            },
            {
                "ProductoId": 1,
                "CantidadProducto": 1,
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

    fetch("https://localhost:44300/api/Pedidos", requestOptions)
        .then(response => response.text())
        .then(result => console.log(result))
        .catch(error => console.log('error', error));
}
export default agregarPedido