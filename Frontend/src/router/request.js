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

function isProfileEnabled () {
  return axios({
    method: 'GET',
    url: `${apiURL}/user/isuseractivated/` + localStorage.getItem('token'),
    headers: {
      'Access-Control-Allow-Origin': '*',
      'Access-Control-Allow-Credentials': true
    },
  })
  .then(response =>{
    if(response.data !== true){
      this.$router.push('/updateprofile')
    }
  })
  .catch(
    this.$router.push('/')
  )
}

export{
  store,
  isProfileEnabled
}
