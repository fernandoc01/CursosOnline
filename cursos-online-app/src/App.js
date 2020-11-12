import React, {useState, useEffect} from 'react';
import {ThemeProvider as MuithemeProvider} from '@material-ui/core/styles';
import theme from './theme/theme';
import Login from './componentes/Seguridad/Login';
import {BrowserRouter as Router, Switch, Route} from 'react-router-dom';
import PerfilUsuario from './componentes/Seguridad/PerfilUsuario';
import RegistrarUsuario from './componentes/Seguridad/RegistrarUsuario';
import {Grid, Snackbar} from '@material-ui/core';
import AppNavBar from './componentes/Navegacion/AppNavBar';
import {useStateValue} from './Contexto/store';
import {obtenerUsuarioActual} from './actions/UsuarioAction';
import RutaSegura from './componentes/Navegacion/RutaSegura'

function App () {
  const [{sesionUsuario, openSnackBar}, dispatch] = useStateValue ();

  const [iniciaApp, setIniciaApp] = useState (false);

  useEffect (
    () => {
      if (!iniciaApp) {
        obtenerUsuarioActual(dispatch)
          .then (response => {
            setIniciaApp (true);
          })
          .catch (error => {
            setIniciaApp (true);
          });
      }
    },
    [iniciaApp]
  );

  return iniciaApp === false ? null : (
    <React.Fragment>
      <Snackbar anchorOrigin={{vertical:"bottom", horizontal:"center"}}
      open = {openSnackBar ? openSnackBar.open : false}
      autoHideDuration={3000}
      ContentProps={{"aria-describedby": "message-id"}}
      message = {
      <span id="message-id">{openSnackBar ? openSnackBar.mensaje : ""}</span>
      }
      onClose={() => {
        dispatch({
          type : "OPEN_SNACKBAR",
          openMensaje : {
            open : false,
            mensaje : " "
          }
        })
      }}>

      </Snackbar>
      <Router>
        <MuithemeProvider theme={theme}>
          <AppNavBar />
          <Grid container>
            <Switch>
              <Route exact path="/auth/login" component={Login} />
              <Route exact path="/auth/registrar" component={RegistrarUsuario} />
              <RutaSegura exact path="/auth/perfil" component={PerfilUsuario}/>
              <RutaSegura exact path="/" component={PerfilUsuario} />
            </Switch>
          </Grid>
        </MuithemeProvider>
      </Router>

    </React.Fragment>
  );
}

export default App;
