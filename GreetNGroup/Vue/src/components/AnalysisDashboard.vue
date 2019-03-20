<template>
  <div class="Dashboard">
    <h2>{{ info }}</h2>
  </div>
</template>

<script>
/* eslint-disable */
import axios from 'axios'

export default {
  name: 'UserAnalysisDashboard',
  data () {
    return {
      info: null
    }
  },
  mounted () {
    axios.get('http://localhost:50884/api/UAD/').then(response => (this.info = response.data))
  }
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
h2 {
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