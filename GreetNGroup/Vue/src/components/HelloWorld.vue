<template>
  <div class="hello">
    <h1>{{ msg }}</h1>
    <h2>{{ info }}</h2>
  </div>
</template>

<script>
import axios from 'axios'

export default {
  name: 'HelloWorld',
  data () {
    return {
      msg: 'Welcome to Your Vue.js App',
      info: null
    }
  },
  mounted () {
    axios.get('https://api.greetngroup.com/api/hello/').then(response => (this.info = response.data))
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
