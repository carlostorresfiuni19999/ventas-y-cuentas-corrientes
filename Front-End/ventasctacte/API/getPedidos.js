export async function getPedidos(token = "") {
    //const myHeaders = new Headers();
    //myHeaders.append("Authorization", `Bearer ${token}` );

    const requestOptions = {
        method: 'GET',
        //headers: myHeaders,
        redirect: 'follow'
        
    };

    return fetch("https://localhost:44300/api/Pedidos", requestOptions)
}

export default getPedidos