<template>
  <div class="Profile" >
     <v-alert
      :value="message"
      dismissible
      type="success"
      transition="scale-transition"
    >
    {{message}}
    </v-alert>
    <v-alert
      :value="errorMessage"
      dismissible
      type="error"
      transition="scale-transition"
    >
    {{errorMessage}}
    </v-alert>
  <div v-if="!userRetrieved">
  <h1>Finding user</h1>
  </div>
  <div v-if="userRetrieved">
    <h1>{{this.json.FirstName + ' ' + this.json.LastName }}</h1>
    <br />
    <h1>Stats:</h1>
    <h2>Events Created:{{this.json.EventCreationCount}}</h2>
    <h2 id="rating">Rating:{{this.json.Rating}}</h2> 

    <v-flex xs12 sm3 id="thumbsUp">
            <v-btn flat icon color="green" v-on:click="submitRating" value="1">
              <v-icon>thumb_up</v-icon>
            </v-btn>
          </v-flex>
    <v-flex xs12 sm3 id="thumbsDown">
            <v-btn flat icon color="red" v-on:click="submitRating" value="-1">
              <v-icon>thumb_down</v-icon>
            </v-btn>
          </v-flex>
          
    <br />
    <h1>Birthday: {{this.json.DoB}}</h1>
    <br />
    <h1>Residence: {{this.json.city + ', ' + this.json.State + ', ' + this.json.Country}}</h1>
    <br />
    </div>
  </div>
  
</template>

<script>
import axios from 'axios'
import { apiURL } from '@/const.js'

export default {
  name: 'Profile',
  data () {
    return {
      userRetrieved: false,
      message: null,
      errorMessage: null,
      userID: this.$route.params.id,
      json: [],
      FirstName: null
    }
  },
  created () {
    axios({
      method: 'GET',
      url: `${apiURL}/user/` + this.userID,
      headers: {
        'Access-Control-Allow-Origin': '*',
        'Access-Control-Allow-Credentials': true
      },
      data: {
        userID: this.userID
      }
    })
      .then(response => (this.json = response.data), this.userRetrieved = true)
      .catch(e => { this.errorMessage = e.response.data })
  },
  methods: {
    submitRating: function (value) {
      axios({
        method: 'POST',
        url: `${apiURL}/user/` + this.userID + '/rate',
        headers: {
          'Access-Control-Allow-Origin': '*',
          'Access-Control-Allow-Credentials': true
        },
        data: {
          jwtToken: this.localStorage.getItem('Token'),
          rating: value
        }
      })
        .then(response => (this.json = response.data))
        .catch(e => { this.errorMessage = e.response.data })
    } 
  }
}
</script>

<style>
.Profile{
  width: 70%;
  margin: 1px auto;
}

#rating {display:inline-block;margin-right:10px;}
#thumbsUp {display:inline-block;} 
#thumbsDown {display:inline-block;} 
</style>
