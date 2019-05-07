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

  </div>
  <div v-if="userRetrieved">
    <h1>{{this.json.FirstName + ' ' + this.json.LastName }}</h1>
    <br />
    <h1>Stats: </h1>
    <h3>Events Created: {{this.json.EventCreationCount}}</h3>
    <h3 id="rating">Rating: {{this.json.Rating}}</h3> 

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
    <h2>Birthday: {{this.json.DoB}}</h2>
    <br />
    <h2>Residence: {{this.json.City + ', ' + this.json.State + ', ' + this.json.Country}}</h2>
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
      json: []
    }
  },
  created () {
    axios({
      method: 'GET',
      url: `${apiURL}/profile/getprofile/` + this.userID,
      headers: {
        'Access-Control-Allow-Origin': '*',
        'Access-Control-Allow-Credentials': true
      },
      data: {
        userID: this.userID
      }
    })
    /* eslint-disable */
      .then(response => (this.json = response.data), this.userRetrieved = true)
      .catch(e => { this.errorMessage = e.response.data, this.userRetrieved = false })
  },
  methods: {
    submitRating: function (value) {
      if (localStorage.getItem('token') !== null) {
        axios({
          method: 'POST',
          url: `${apiURL}/user/` + this.userID + '/rate',
          headers: {
            'Access-Control-Allow-Origin': '*',
            'Access-Control-Allow-Credentials': true
          },
          data: {
            jwtToken: localStorage.getItem('Token'),
            rating: value
          }
        })
          .then(response => (this.json = response.data))
          .catch(e => { this.errorMessage = e.response.data })
      } else {
        this.errorMessage = 'Must be logged in to rate user'
      }
    }
  }
}
</script>

<style>
.Profile{
  text-align: left;
  width: 70%;
  margin: 1px auto;
}

#rating {display:inline-block;margin-right:10px;}
#thumbsUp {display:inline-block;} 
#thumbsDown {display:inline-block;} 
</style>
