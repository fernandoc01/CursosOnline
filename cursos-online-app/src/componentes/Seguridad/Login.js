import React, { useState } from 'react';
import style from '../Tool/style'
import {Container, Avatar, Typography, TextField, Button} from '@material-ui/core'
import LockOutlinedIcon from '@material-ui/icons/LockOutlined'
import { loginUsuario } from '../../actions/UsuarioAction';
import { withRouter } from 'react-router-dom';
import { useStateValue } from '../../Contexto/store';

const Login = props => {

    const[{sesionUsuario}, dispatch] = useStateValue()

    const [usuario, setUsuario] = useState({
        Email : '',
        Password : ''
    })

    const guardarValoresEnMemoria = e => {
        const{name, value} = e.target
        setUsuario(anterior => ({
            ...anterior,
            [name] : value
        }))
    }

    const userLogin = e => {
        e.preventDefault()
        loginUsuario(usuario, dispatch).then(response => {

            if(response.status === 200){
                window.localStorage.setItem("token_seguridad", response.data.token)
                props.history.push('/')
            }else{
                dispatch({
                    type: "OPEN_SNACKBAR",
                    openMensaje: {
                    open: true,
                    mensaje: "Las credenciales del usuario son incorrectas"
                }})
            }
            
            
        })
    }
    
    return(
        <Container maxWidth="xs">
            <div style={style.paper}>
                <Avatar style={style.avatar}>
                    <LockOutlinedIcon style={style.icon}/>
                </Avatar>
                <Typography component="h1" variant="h5">
                    Login de Usuario
                </Typography>
                <form style={style.form}>
                    <TextField name="Email" onChange={guardarValoresEnMemoria} variant="outlined" label="Ingrese Email" fullWidth margin="normal"/>
                    <TextField name="Password" onChange={guardarValoresEnMemoria} variant="outlined" type="password" label="Ingrese Contraseña" fullWidth margin="normal"/>
                    <Button type="submit" onClick={userLogin} fullWidth variant="contained" color="primary" style={style.submit}>Enviar</Button>
                </form>
            </div>
        </Container>
    )
}

export default withRouter( Login );