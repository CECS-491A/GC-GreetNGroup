<template>
  <div class="Welcome">
    <h1>Welcome</h1>
    <br />
    <br />
  </div>
</template>

<script>
import { isProfileEnabled } from '@/router/request'

export default {
  
  name: 'Welcome',
  data () {
    return {
    }
  },
  created () {
    isProfileEnabled()
  }
}
</script>

<style>
.Welcome{
  width: 70%;
  margin: 1px auto;
}
</style>
