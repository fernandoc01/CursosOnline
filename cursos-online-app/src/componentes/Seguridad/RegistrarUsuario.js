import React, {useState} from 'react';
import {
  Container,
  Typography,
  Grid,
  TextField,
  Button,
} from '@material-ui/core';
import style from '../Tool/style';
import {actionRegistrarUsuario} from '../../actions/UsuarioAction'

const RegistrarUsuario = () => {
  const [usuario, setUsuario] = useState ({
    NombreCompleto: '',
    Email: '',
    UserName: '',
    Password: '',
    confirmarPassword: ''
  });

  const guardarValoresEnMemoria = e => {
    const {name, value} = e.target;

    setUsuario (anterior => ({
      ...anterior,
      [name]: value,
    }));
  };

  const btnRegistrarUsuario = e => {
    e.preventDefault()
    //console.log(usuario)
    actionRegistrarUsuario(usuario).then(response => {
      console.log("Registro de usuario exitoso!!!", response)
      window.localStorage.setItem("token_seguridad", response.data.token)
    })
  }

  return (
    <Container component="main" maxWidth="md" justify="center">
      <div style={style.paper}>
        <Typography component="h1" variant="h5">
          Registro de Usuario
        </Typography>
        <form style={style.form}>
          <Grid container spacing={2}>
            <Grid item xs={12} md={12}>
              <TextField
                name="NombreCompleto"
                value={usuario.NombreCompleto}
                onChange={guardarValoresEnMemoria}
                variant="outlined"
                fullWidth
                label="Ingrese su nombre completo"
              />
            </Grid>
            <Grid item xs={12} md={6}>
              <TextField
                name="Email"
                value={usuario.Email}
                onChange={guardarValoresEnMemoria}
                variant="outlined"
                fullWidth
                label="Ingrese su Email"
              />
            </Grid>
            <Grid item xs={12} md={6}>
              <TextField
                name="UserName"
                value={usuario.UserName}
                onChange={guardarValoresEnMemoria}
                variant="outlined"
                fullWidth
                label="Ingrese el Nombre de usuario"
              />
            </Grid>
            <Grid item xs={12} md={6}>
              <TextField
                name="Password"
                value={usuario.Password}
                onChange={guardarValoresEnMemoria}
                type="Password"
                variant="outlined"
                fullWidth
                label="Ingrese la contraseña"
              />
            </Grid>
            <Grid item xs={12} md={6}>
              <TextField
                name="confirmarPassword"
                value={usuario.confirmarPassword}
                onChange={guardarValoresEnMemoria}
                type="Password"
                variant="outlined"
                fullWidth
                label="Confirmar contraseña"
              />
            </Grid>
          </Grid>
          <Grid container justify="center">
            <Grid item xs={12} md={6} />
            <Button
              onClick={btnRegistrarUsuario}
              type="submit"
              fullWidth
              variant="contained"
              color="primary"
              size="large"
              style={style.submit}
            >
              Enviar
            </Button>

          </Grid>
        </form>
      </div>
    </Container>
  );
};

export default RegistrarUsuario;
