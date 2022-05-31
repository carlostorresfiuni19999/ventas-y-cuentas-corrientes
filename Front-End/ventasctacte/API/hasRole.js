const hasRole = (token, email, role) =>{
    const myHeaders = new Headers();
    myHeaders.append("Authorization", `Bearer ${token}` );

    const link = `https://cuentasctacte-web-api20220425205158.azurewebsites.net/api/Account/HasRole?email=${email}&role=${role}`
    
    const requestOptions = {
        method: 'GET',
        headers: myHeaders,
        redirect: 'follow'
        
    };

    return fetch(link, requestOptions)
        .then(result=>result.text())
        .then(result=>alert(JSON.parse(result)))
        .catch(error => console.log('error', error));
}
export default hasRole