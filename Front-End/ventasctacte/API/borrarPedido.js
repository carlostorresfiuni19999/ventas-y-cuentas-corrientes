const borrarPedido = (token, id) => {
    const myHeaders = new Headers();
    myHeaders.append("Authorization", `Bearer ${token}`);

    const requestOptions = {
        method: 'DELETE',
        headers: myHeaders,
        redirect: 'follow'
    };

    return fetch(`https://cuentasctacte-web-api20220425205158.azurewebsites.net/api/Pedidos?Id=${id}`, requestOptions)
        .then(response => response.text())
        .then(result => console.log(result))
        .catch(error => console.log('error', error));
}

export default borrarPedido