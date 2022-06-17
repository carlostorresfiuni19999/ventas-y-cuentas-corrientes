import { Router } from "next/router";

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
        .then(response => {
            if(response.ok){
                alert("Agregado con exito")
                if(confirm("volver a la lista de notas de pedido?")){   
                    Router.push('/ndp/Lista')
                }else{
                    Router.push('/ndp/Agregar')
                }
            }else{
                response.json().then(error => {alert(error.Message)});
            }
        }).catch(error => console.log(error));
}
export default agregarPedido