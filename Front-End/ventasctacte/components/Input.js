import React from 'react'
import {ErrorMessage, Field, ImportMessage} from 'formik'
import TextError from './TextError'
import styles from '../styles/LogIn.module.css'

function Input(props){
    const{label, name, ...resto} = props
    return(
        <div className='form-control'>
            <div>
                <label htmlFor={name} className={styles.labels}>{label}</label>
            </div>
            <Field id={name} name = {name} {...resto}/>
            <ErrorMessage name={name} component={TextError} />
        </div>
    )
}    

export default Input