const postOrden = (token, id) => {
    const myHeaders = new Headers();
    myHeaders.append("Authorization", `Bearer ${token}`);

    const requestOptions = {
        method: 'POST',
        headers: myHeaders,
        redirect: 'follow'
    };

    return fetch(`https://cuentasctacte-web-api20220425205158.azurewebsites.net/api/Pagos/OrdenDeCobro?FacturaId=${id}`, requestOptions)
        .then(response => response.text())
        .then(result => console.log(result))
        .catch(error => alert('error', error));
}
export default postOrden