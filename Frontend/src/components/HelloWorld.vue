<template>
  <div class="hello">
    <h1>{{ msg }}</h1>
    <h2>{{ info }}</h2>
    <input type="text" v-model="search" placeholder="" />
  </div>
</template>

<script>
import axios from 'axios'
import { apiURL } from '@/const.js'

export default {
  name: 'HelloWorld',
  data () {
    return {
      msg: 'GreetNGroup',
      info: null,
      search: ''
    }
  },
  mounted () {
    axios.get(`${apiURL}/helloworld/hello`).then(response => (this.info = response.data))
    console.log(process.env.VUE_APP_API_URL)
  }
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
h1, h2 {
  font-weight: normal;
}
ul {
  list-style-type: none;
  padding: 0;
}
li {
  display: inline-block;
  margin: 0 10px;
}
a {
  color: #42b983;
}
</style>
