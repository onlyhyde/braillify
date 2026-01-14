import { Center, Text } from '@devup-ui/react'
import { ComponentProps } from 'react'

interface TestCaseStatProps extends ComponentProps<typeof Center<'div'>> {
  showTotal?: boolean
  total: number
  success: number
  fail: number
}

export function TestCaseStat({
  showTotal = false,
  total,
  success,
  fail,
  ...props
}: TestCaseStatProps) {
  const hasFail = fail > 0

  return (
    <Center
      bg="$menuHover"
      borderRadius="10px"
      gap="10px"
      px="16px"
      py="10px"
      styleOrder={1}
      {...props}
    >
      {showTotal && (
        <Text color="$text" typography="progress">
          전체 {total.toLocaleString()}
        </Text>
      )}
      <Text color="$text" typography="progress">
        성공 {success.toLocaleString()}
      </Text>
      <Text color={hasFail ? '$error' : '$text'} typography="progress">
        실패 {fail.toLocaleString()}
      </Text>
      <Text
        color={showTotal ? '$text' : hasFail ? '$error' : '$success'}
        typography="progress"
      >
        ({Math.round((success / total) * 100)}%)
      </Text>
    </Center>
  )
}
