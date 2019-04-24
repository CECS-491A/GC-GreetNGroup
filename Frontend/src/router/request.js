/*
import Vue from 'vue'
import Vuex from 'vuex'

Vue.use(Vuex)

export const store = new Vuex.Store({
  state: {
    email: '',
    status: '',
    token: localStorage.getItem('Token') || ''
  },
  getters: {
    getEmail: state => {
      return state.email
    },
    isLoggedIn: state => {
      return !!state.token
    },
    isAuthenticated: state => {
      return state.status
    }
  },
  mutations: {
    authentication_success (state, token, email) {
      state.status = 'success'
      state.token = token
      state.email = email
    },
    authentication_invalid (state) {
      state.status = 'unsuccessful'
    },
    logout (state) {
      state.status = ''
      state.token = ''
      state.email = ''
    }
  }
})
*/

import axios from 'axios'
import { apiURL } from '@/const.js'

/* eslint-disable */
let token = localStorage.getItem('token');

const store = {
  state: {
    isLogin: false,
    email: ""
  },
  isUserLogin(){
    if(token !== null){
      this.state.isLogin = true;
    }
    else{
      this.state.isLogin= false;
    }
  },
  getEmail(){
    axios({
      method: 'POST',
      url: `${apiURL}/user/email/getemail`,
      headers: {
        'Access-Control-Allow-Origin': '*',
        'Access-Control-Allow-Credentials': true
      },
      data: {
        token: localStorage.getItem('token')
      }
    })
      .then(response => (this.state.email = response.data))
      .catch(e => {this.state.email = '', this.state.isLogin = false, localStorage.removeItem('token')})
  }
};

export{
  store
}
