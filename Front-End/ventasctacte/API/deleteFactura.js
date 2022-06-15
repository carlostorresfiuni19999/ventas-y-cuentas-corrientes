const deleteFactura = (token, id) => {
    const myHeaders = new Headers();
    myHeaders.append("Authorization", `Bearer ${token}`);

    const requestOptions = {
        method: 'DELETE',
        headers: myHeaders,
        redirect: 'follow'
    };

    return fetch(`https://cuentasctacte-web-api20220425205158.azurewebsites.net/api/Facturas/${id}`, requestOptions)
        .then(response => {
            if(response.ok){
                alert("Eliminado con exito");
            }else{
                response.json().then(error => {alert(error.Message)});
            }
        }).catch(error => console.log(error));
}

export default deleteFactura