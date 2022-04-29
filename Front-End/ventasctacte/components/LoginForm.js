import React from 'react'
import { useFormik } from "formik";
import * as Yup from "yup";
import styles from '../styles/LogIn.module.css'
import login from "../API/login"
import { useRouter } from 'next/router';

function LoginForm() {
    const router = useRouter()
    const formik = useFormik({
        initialValues: {
            user: '',
            pass: ''
        },
        onSubmit: (values) => {
            console.log("Hola mundo")
            login(values.user, values.pass)
            .catch(error => {
                console.log('error', error);
                alert("Error al intentar ingresar. Verifica las credenciales");
            })
                .then(response => response.text())
                .then(result => {
                    console.log(result);
                    const res = JSON.parse(result)

                    if(res.error_description){
                        alert("Error, Credenciales no validas");
                    }else{
                        sessionStorage.setItem("token", JSON.stringify(res));
                        alert("Logeado con exito");
                        router.push("ndp/Lista")
                    }
                    
                    
                }
                )

        },
        validacion: Yup.object({
            user: Yup.string().required('*Obligatorio'),
            //pass: Yup.string().required('*Obligatorio').matches(/^.*(?=.{8,})((?=.*[!@#$%^&*()\-_=+{};:,<.>]){1})(?=.*\d)((?=.*[a-z]){1})((?=.*[A-Z]){1}).*$/,
            //    "La contraseña debe contener al menos 8 letras, uno Mayuscula un numero y una figura")
            pass: Yup.string().required('*Obligatorio')
        }
        )
    })

    return (


        /*  <div className={styles.LoginPanel}>
             <h1 className={styles.letraGrande}>Login</h1>
             
             
                 formik => {
                     return <Form onSubmit={formik.handleSubmit}>
                         <FormikControl
                             control='input'
                             type='text'
                             label="Usuario"
                             name='user'
 
                         />
                         <FormikControl
                             control='input'
                             type='password'
                             label="Contraseña"
                             name='pass'
                         />
                         <button type="submit" className={styles.boton} disabled={!formik.isValid}> Login </button>
                     </Form>
                 }
             }
         </Formik>
         </div >
         */

        <div className={styles.LoginPanel}>
            <h1 className={styles.letraGrande}>Login</h1>
            <form onSubmit={formik.handleSubmit}>
                <div>
                <label htmlFor='user'>
                    Email
                </label>
                <input
                    id="user"
                    name="user"
                    type="email"
                    onChange={formik.handleChange}
                    value={formik.values.user}
                />
                </div>
                <div>
                <label htmlFor='password'>
                    Password
                </label>

                <input
                    id="pass"
                    name="pass"
                    type="password"
                    onChange={formik.handleChange}
                    value={formik.values.pass}
                />
                </div>
                <button type="submit" className={styles.boton} disabled={!formik.isValid}> Login </button>
            </form>
        </div>
    );
}

export default LoginForm