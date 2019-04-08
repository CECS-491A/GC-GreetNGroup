<template>
  <div class="UserAnalytics">
    <h1 class='display-2'>User Analytics </h1>

    <div>
    <input v-model='input' type="text" placeholder="Search for month">
    </div>
    <div>
    <button @click="searchInput()">Search</button>

    <v-container fluid grid-list-md>
    <v-layout row wrap>
      <v-flex d-flex xs12 sm6 md4>
        <v-card color="purple" dark>
          <v-card-title primary class="justify-center" style = "font-size: 20px; text-decoration: underline;">Login VS Registered Users</v-card-title>
          <v-card-text>{{logvsreg}}</v-card-text>
        </v-card>
      </v-flex>
      <v-flex d-flex xs12 sm6 md4>
        <v-card color="purple" dark>
          <v-card-title primary class="justify-center" style = "font-size: 20px; text-decoration: underline;">Average Session Time (minutes)</v-card-title>
          <v-card-text>{{avgsession}}</v-card-text>
        </v-card>
      </v-flex>
      <v-flex d-flex xs12 sm6 md4>
        <v-card color="purple" dark>
          <v-card-title primary class="justify-center" style = "font-size: 20px; text-decoration: underline;">Top 5 Features</v-card-title>
          <v-card-text>{{top5feature}}</v-card-text>
        </v-card>
      </v-flex>
    </v-layout>
  </v-container>

   <v-container fluid grid-list-md>
    <v-layout row wrap>
      <v-flex d-flex xs12 sm6 md4>
        <v-card color="purple" dark>
          <v-card-title primary class="justify-center" style = "font-size: 20px; text-decoration: underline;">Top 5 Pages</v-card-title>
          <v-card-text>{{top5pages}}</v-card-text>
        </v-card>
      </v-flex>
      <v-flex d-flex xs12 sm6 md4>
        <v-card color="purple" dark>
          <v-card-title primary class="justify-center" style = "font-size: 20px; text-decoration: underline;">Logins Monthly</v-card-title>
          <v-card-text>{{loginmonth}}</v-card-text>
        </v-card>
      </v-flex>
      <v-flex d-flex xs12 sm6 md4>
        <v-card color="purple" dark>
          <v-card-title primary class="justify-center" style = "font-size: 20px; text-decoration: underline;">Avg Session per Month (minutes)</v-card-title>
          <v-card-text>{{sessionmonthly}}</v-card-text>
        </v-card>
      </v-flex>
    </v-layout>
  </v-container>
    </div>
  </div>
</template>

<script>
import axios from 'axios'

export default {
  name: 'UserAnalytics',
  data () {
    return {
      input: '',
      logvsreg: '',
      avgsession: '',
      top5feature: '',
      top5pages: '',
      loginmonth: '',
      sessionmonthly: ''
    }
  },

  methods: {
    searchInput: function () {
      axios.get('http://localhost:62008/api/UAD/LoginVSRegistered/' + this.input).then((response) => { this.logvsreg = response.data }).catch(error => console.log(error))
      axios.get('http://localhost:62008/api/UAD/AverageSessionDuration/' + this.input).then((response) => { this.avgsession = response.data }).catch(error => console.log(error))
      axios.get('http://localhost:62008/api/UAD/GetTop5MostUsedFeature/' + this.input).then((response) => { this.top5feature = response.data }).catch(error => console.log(error))
      axios.get('http://localhost:62008/api/UAD/Top5AveragePageSession/' + this.input).then((response) => { this.top5pages = response.data }).catch(error => console.log(error))
      axios.get('http://localhost:62008/api/UAD/LoggedInMonthly/' + this.input).then((response) => { this.loginmonth = response.data }).catch(error => console.log(error))
      axios.get('http://localhost:62008/api/UAD/AverageSessionMonthly/' + this.input).then((response) => { this.sessionmonthly = response.data }).catch(error => console.log(error))
    }
  } 
    
}
</script>

<style>
h1{
  padding-top: 10px;
  padding-right: 20px;
  padding-bottom: 20px;
  padding-left: 20px;
  text-align: center;
}

ul {
  padding:0;
  margin:0;
  display: inline-block;
  text-align:left;
}
</style>
