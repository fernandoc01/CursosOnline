import React from 'react';
import ReactDOM from 'react-dom';
import './index.css';
import App from './App';
import reportWebVitals from './reportWebVitals';
import {initialState} from './Contexto/InitialState';
import {StateProvider} from './Contexto/store';
import {mainReducer} from './Contexto/reducers'

ReactDOM.render(
  <React>
    <StateProvider initialState={initialState} reducer={mainReducer}>
    <App />
    </StateProvider>
    
  </React>,
  document.getElementById('root')
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
