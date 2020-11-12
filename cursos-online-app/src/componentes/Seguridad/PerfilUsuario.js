import React, { useEffect, useState } from 'react';
import style from '../Tool/style'
import {Container, Typography, Grid, TextField, Button, Avatar} from '@material-ui/core';
import { actualizarUsuario } from '../../actions/UsuarioAction';
import { useStateValue } from '../../Contexto/store';
import reactFoto from '../../logo.svg';
import {v4 as uuidv4} from 'uuid';
import ImageUploader from 'react-images-upload';
import { ObtenerDataImagen } from '../../actions/ImagenAction';

const PerfilUsuario = () => {

    const [{sesionUsuario}, dispatch] = useStateValue()

    const [usuario, setUsuario] = useState({
        nombreCompleto : '',
        email : '',
        Password : '',
        ConfirmePassword : '',
        userName: '',
        imagenPerfil: null,
        fotoUrl: ''
    })

    const guardarValoresEnMemoria = e => {
        const{name, value} = e.target
        setUsuario(anterior => ({
            ...anterior,
            [name] : value
        }))
    }

    useEffect(() => {
        /*obtenerUsuarioActual(dispatch).then(response => {
            setUsuario(response.data)
        })*/
        setUsuario(sesionUsuario.usuario)
        setUsuario(anterior => ({
            ...anterior,
            Password: '',
            ConfirmePassword: '',
            imagenPerfil: null,
            fotoUrl: sesionUsuario.usuario.imagenPerfil 
        }))
    }, [sesionUsuario.usuario])  

    const btnActualizarUsuario = e => {
        e.preventDefault()
        actualizarUsuario(usuario, dispatch).then(response => {

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
                        mensaje : "Hubo errores al intentar guardar en: " + Object.keys(response.data.errors)                    
                    }
                })
            }

            
            
        })
    }

    const fotoKey = uuidv4();

    const subirFoto = imagenes => {
        const foto = imagenes[0]
        const fotoUrl = URL.createObjectURL(foto)

        ObtenerDataImagen(foto).then(respuesta => {
            console.log(respuesta)
            setUsuario(anterior => ({
                ...anterior,
                imagenPerfil : respuesta, //archivo en formato file
                fotoUrl : fotoUrl //archivo en formato url
            }))

        })

        
    }

    return(
        <Container component="main" maxWidth="md" justify="center">
            <div style={style.paper}>
                <Avatar style={style.avatar} src={usuario.fotoUrl || reactFoto}/>
                <Typography component="h1" variant="h5">
                    Perfil de {sesionUsuario.usuario.nombreCompleto}
                </Typography>
            
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
                    <Grid item xs={12} md={12}>
                        <ImageUploader
                            withIcon = {false}
                            key = {fotoKey}
                            singleImage = {true}
                            buttonText = "Seleccione una imagen de perfil"
                            onChange = {subirFoto}
                            imgExtension = {[".jpg", ".png", ".gif", ".jpeg"]}
                            maxFileSize = {5242880}
                        />
                    </Grid>
                </Grid>
                <Grid container spacing={2}>
                    <Grid item xs={12} md={12}>
                        <Button type="submit" onClick={btnActualizarUsuario} style={style.submit} size="large" color="primary" variant="contained" fullWidth>Guardar Datos</Button>
                    </Grid>
                </Grid>
            </form>
            </div>
        </Container>
    )
}

export default PerfilUsuario;