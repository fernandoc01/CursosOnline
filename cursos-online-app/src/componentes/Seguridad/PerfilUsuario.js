import React, { useEffect, useState } from 'react';
import style from '../Tool/style'
import {Container, Typography, Grid, TextField, Button} from '@material-ui/core';
import { actualizarUsuario, obtenerUsuarioActual } from '../../actions/UsuarioAction';
import { useStateValue } from '../../Contexto/store';


const PerfilUsuario = () => {

    const [{sesionUsuario}, dispatch] = useStateValue()

    const [usuario, setUsuario] = useState({
        nombreCompleto : '',
        email : '',
        Password : '',
        ConfirmePassword : '',
        userName: ''
    })

    const guardarValoresEnMemoria = e => {
        const{name, value} = e.target
        setUsuario(anterior => ({
            ...anterior,
            [name] : value
        }))
    }

    useEffect(() => {
        obtenerUsuarioActual(dispatch).then(response => {
            console.log("Data del usuario actual", response)
            setUsuario(response.data)
        })
    }, [])

    const btnActualizarUsuario = e => {
        e.preventDefault()
        actualizarUsuario(usuario).then(response => {

            if(response.status===200){
                dispatch({
                    type : "OPEN_SNACKBAR",
                    openMensaje : {
                        open : true, 
                        mensaje : "Se guardaron exitosamente los datos del perfil de usuario"
                    }
                })

                window.localStorage.setItem("token_seguridad", response.data.token)
                
            }else{
                dispatch({
                    type : "OPEN_SNACKBAR",
                    openMensaje : {
                        open : true, 
                        mensaje : "Hubo errores al intentar guardar en: " + Object.keys(response.data.errors)                    }
                })
            }

            
            
        })
    }

    return(
        <Container component="main" maxWidth="md" justify="center">
            <div style={style.paper}>
                <Typography component="h1" variant="h5">
                    Perfil de Usuario
                </Typography>
            </div>
            <form style={style.form}>
                <Grid container spacing={2}>
                    <Grid item xs={12} md={12}>
                        <TextField name="nombreCompleto" value={usuario.nombreCompleto} onChange={guardarValoresEnMemoria} variant="outlined" fullWidth label="Ingrese Nombre y Apellido"/>
                    </Grid>
                    <Grid item xs={12} md={6}>
                        <TextField name="userName" value={usuario.userName} onChange={guardarValoresEnMemoria} variant="outlined" fullWidth label="Ingrese Nombre de Usuario"/>
                    </Grid>
                    <Grid item xs={12} md={6}>
                        <TextField name="email" value={usuario.email} onChange={guardarValoresEnMemoria} variant="outlined" fullWidth label="Ingrese email"/>
                    </Grid>
                    <Grid item xs={12} md={6}>
                        <TextField name="Password" value={usuario.Password} onChange={guardarValoresEnMemoria} type="password" variant="outlined" fullWidth label="Ingrese Contraseña"/>
                    </Grid>
                    <Grid item xs={12} md={6}>
                        <TextField name="ConfirmePassword" value={usuario.ConfirmePassword} onChange={guardarValoresEnMemoria} type="password" variant="outlined" fullWidth label="Confirmar Contraseña"/>
                    </Grid>
                </Grid>
                <Grid container spacing={2}>
                    <Grid item xs={12} md={12}>
                        <Button type="submit" onClick={btnActualizarUsuario} style={style.submit} size="large" color="primary" variant="contained" fullWidth>Guardar Datos</Button>
                    </Grid>
                </Grid>
            </form>
        </Container>
    )
}

export default PerfilUsuario;