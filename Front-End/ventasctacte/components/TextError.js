import React from 'react'
import styles from '../styles/LogIn.module.css'

function TextError(props){
    return(
        <div className={styles.Error}>
            {props.children}
        </div>
    )
}

export default TextError