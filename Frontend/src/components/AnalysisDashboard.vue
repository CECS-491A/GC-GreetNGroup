<template>
  <div class="UserAnalytics">
    <h1 class='display-2'>User Analytics </h1>

    <div>
    <input v-model='month' type="text" :maxlength=15 placeholder="Search for a month">
    <input v-model='year' type="text" :maxlength=4 placeholder="Search for year">
    </div>
    <div>
    <button @click="searchInput(month, year)">Search</button>

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
      sessionmonthly: '',
      month: '',
      year: ''
    }
  },

  methods: {
    checkMonth: function (i) {
      var months = [ 'January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December' ]
      i = i.charAt(0).toUpperCase() + i.slice(1)
      if (months.indexOf(i) !== -1) {
        return true
      }
      return false
    },
    checkYear: function (i) {
      return (Number.isInteger(+i) && (+i > 0)) 
    },
    searchInput: function (month, year) {
      var checkMonth = this.checkMonth(month)
      var checkYear = this.checkYear(year)
      if (checkYear === true && checkMonth === true) {
        month = month.charAt(0).toUpperCase() + month.slice(1)
        axios.get('http://localhost:62008/api/UAD/LoginVSRegistered/' + month + '/' + year).then((response) => { this.logvsreg = response.data }).catch(error => console.log(error))
        axios.get('http://localhost:62008/api/UAD/AverageSessionDuration/' + month + '/' + year).then((response) => { this.avgsession = response.data }).catch(error => console.log(error))
        axios.get('http://localhost:62008/api/UAD/GetTop5MostUsedFeature/' + month + '/' + year).then((response) => { this.top5feature = response.data }).catch(error => console.log(error))
        axios.get('http://localhost:62008/api/UAD/Top5AveragePageSession/' + month + '/' + year).then((response) => { this.top5pages = response.data }).catch(error => console.log(error))
        axios.get('http://localhost:62008/api/UAD/LoggedInMonthly/' + month + '/' + year).then((response) => { this.loginmonth = response.data }).catch(error => console.log(error))
        axios.get('http://localhost:62008/api/UAD/AverageSessionMonthly/' + month + '/' + year).then((response) => { this.sessionmonthly = response.data }).catch(error => console.log(error)) 
      } else {
        alert('Not a valid Date')
      } 
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
