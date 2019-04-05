<template>
  <div class="UserAnalytics">
    <h1>User Analytics </h1>
      <p v-if="data.length">
          <ul>
            <li v-for="(item, index) in data" :key="index">
              {{ item }}
            </li>
          </ul>
      </p>
  </div>
</template>

<script>
import axios from 'axios'

export default {
  name: 'UserAnalytics',
  data () {
    return {
      data: []
    }
  },
  mounted () {
    axios.get('https://api.greetngroup.com/api/UAD/' + 6).then((response) => { this.data.push(response.data) }).catch(error => console.log(error))
    axios.get('https://api.greetngroup.com/api/UAD/' + 5).then((response) => { this.data.push(response.data) }).catch(error => console.log(error))
    axios.get('https://api.greetngroup.com/api/UAD/' + 4).then((response) => { this.data.push(response.data) }).catch(error => console.log(error))
    axios.get('https://api.greetngroup.com/api/UAD/' + 3).then((response) => { this.data.push(response.data) }).catch(error => console.log(error))
    axios.get('https://api.greetngroup.com/api/UAD/' + 2).then((response) => { this.data.push(response.data) }).catch(error => console.log(error))
    axios.get('https://api.greetngroup.com/api/UAD/' + 1).then((response) => { this.data.push(response.data) }).catch(error => console.log(error))
  }
}
</script>

<style>
.UserAnalytics{
  padding: 70px 0;
  text-align: center;
}

ul {
  padding:0;
  margin:0;
  display: inline-block;
  text-align:left;
}
</style>
