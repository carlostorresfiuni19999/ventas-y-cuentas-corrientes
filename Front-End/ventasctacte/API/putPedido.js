export async function putPedido(token, id, raw) {
    const myHeaders = new Headers();
    myHeaders.append("Authorization", `Bearer ${token}` );
    myHeaders.append("Content-Type", "application/json");

    const link = `https://cuentasctacte-web-api20220425205158.azurewebsites.net/api/Pedidos/${id}`
    const requestOptions = {
        method: 'PUT',
        headers: myHeaders,
        body: raw,
        redirect: 'follow'
        
    };

    return fetch(link, requestOptions)
        .then(response => {
            if(response.ok){
                alert("Editado con exito");
            }else{
                response.json().then(error => {alert(error.Message)});
            }
        }).catch(error => console.log(error));
}

export default putPedido