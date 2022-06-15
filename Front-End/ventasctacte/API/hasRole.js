const hasRole =  (token, email, role) =>{
    const myHeaders = new Headers();
    myHeaders.append("Authorization", `Bearer ${token}` );

    const link = `https://cuentasctacte-web-api20220425205158.azurewebsites.net/api/Account/HasRole?email=${email}&role=${role}`
    
    const requestOptions = {
        method: 'GET',
        headers: myHeaders,
        redirect: 'follow'
        
    };

    return fetch(link, requestOptions).then(r =>  r.text());
}
export default hasRole