
import hasRole from "./hasRole";


const ValidateLogin = (token, role, validate = ()=>{}, reject = () => {}) => {
    if(token){
        const access = token.access_token;
        const email = token.userName;
        hasRole(access, email, role)
        .then(r => {
            r ? validate() : reject();
        })
        .catch(e => console.log(e));
        
    } else reject();
}

export  default ValidateLogin;