import {
  Avatar,
  Button,
  Drawer,
  IconButton,
  makeStyles,
  Toolbar,
  Typography,
} from '@material-ui/core';
import React, {useState} from 'react';
import FotoUsuarioTemp from '../../../logo.svg';
import {useStateValue} from '../../../Contexto/store';
import {MenuIzquierda} from './MenuIzquierda';
import {MenuDerecha} from './MenuDerecha';
import {withRouter} from 'react-router-dom';

const useStyles = makeStyles (theme => ({
  seccionDesktop: {
    display: 'none',
    [theme.breakpoints.up ('md')]: {
      display: 'flex',
    },
  },
  seccionMobile: {
    display: 'flex',
    [theme.breakpoints.up ('md')]: {
      display: 'none',
    },
  },
  grow: {
    flexGrow: 1,
  },
  avatarSize: {
    width: 40,
    height: 40,
  },
  list: {
    width: 250,
  },
  listItemText: {
    fontSize: '14px',
    fontWeight: 600,
    paddingLeft: '15px',
    color: '#212121',
  },
}));

const BarSesion = props => {
  const classes = useStyles ();
  const [{sesionUsuario}, dispatch] = useStateValue ();
  const [abrirMenuIzquierda, setAbrirMenuIzquierda] = useState (false);
  const [abrirmenuDerecha, setAbrirMenuDerecha] = useState (false);

  const cerrarMenuIzquierda = () => {
    setAbrirMenuIzquierda (false);
  };

  const btnAbrirMenuIzquierda = () => {
    setAbrirMenuIzquierda (true);
  };

  const cerrarMenuDerecha = () => {
    setAbrirMenuDerecha (false);
  };

  const btnAbrirMenuDerecha = () => {
    setAbrirMenuDerecha (true);
  };

  const salirSesionApp = () => {
    localStorage.removeItem ('token_seguridad');
    props.history.push ('/auth/login');

    dispatch({
      type: "SALIR_SESION",
      nuevoUsuario: null,
      autenticado: false
    })
  };

  return (
    <React.Fragment>

      <Drawer
        open={abrirMenuIzquierda}
        onClose={cerrarMenuIzquierda}
        anchor="left"
      >
        <div
          className={classes.list}
          onKeyDown={cerrarMenuIzquierda}
          onClick={cerrarMenuIzquierda}
        >
          <MenuIzquierda classes={classes} />
        </div>
      </Drawer>

      <Drawer
        open={abrirmenuDerecha}
        onClose={cerrarMenuDerecha}
        anchor="right"
      >
      <div
        className={classes.list}
        onClick={cerrarMenuDerecha}
        onKeyDown={cerrarMenuDerecha}
      >
        <MenuDerecha classes={classes} 
        salirSesion={salirSesionApp} 
        usuario = {sesionUsuario ? sesionUsuario.usuario : null}/>
      </div>
      </Drawer>

      <Toolbar>
        <IconButton color="inherit" onClick={btnAbrirMenuIzquierda}>
          <i className="material-icons">menu</i>
        </IconButton>
        <Typography variant="h6">Cursos Online</Typography>
        <div className={classes.grow} />

        <div className={classes.seccionDesktop}>
          <Button color="inherit" onClick={salirSesionApp}>
            Salir
          </Button>
          <Button color="inherit">
            {sesionUsuario ? sesionUsuario.usuario.nombreCompleto : ''}
          </Button>
          <Avatar src={sesionUsuario.usuario.imagenPerfil || FotoUsuarioTemp} />
        </div>

        <div className={classes.seccionMobile}>
          <IconButton color="inherit" onClick={btnAbrirMenuDerecha}>
            <i className="material-icons">more_vert</i>
          </IconButton>
        </div>
      </Toolbar>

    </React.Fragment>
  );
};

export default withRouter (BarSesion);
