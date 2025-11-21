import { Box, Flex } from '@devup-ui/react'

import { LeftMenu } from './LeftMenu'
import { RightIndex } from './RightIndex'

export default function DocsLayout({
  children,
}: Readonly<{
  children: React.ReactNode
}>) {
  return (
    <Flex maxW="1520px" minH="calc(100vh - 500px)" mx="auto" w="100%">
      <Box display={['none', null, 'initial']} minW="220px" p="20px 16px">
        <Box pos="sticky" top={['70px', null, '90px']}>
          <LeftMenu />
        </Box>
      </Box>
      <Box
        className="markdown-body"
        color="$text"
        px={['16px', null, null, '60px']}
        py={['24px', null, null, '40px']}
        w="100%"
      >
        {children}
      </Box>
      <Box display={['none', null, null, 'initial']}>
        <Box pos="sticky" top={['50px', null, '70px']}>
          <RightIndex />
        </Box>
      </Box>
    </Flex>
  )
}
